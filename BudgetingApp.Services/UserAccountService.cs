using Budget.Adapters;
using BudgetingApp.Adapter.Constants;
using BudgetingApp.Adapter.Data;
using BudgetingApp.Common.Results;
using BudgetingApp.Infrastructure.Data;
using BudgetingApp.Infrastructure.Repositories;
using BudgetingApp.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Services
{
    public interface IUserAccountService
    {
        Task<Result> CreateUserAsync(string name, string email, string username);

        Task<Result<string>> CreateLinkAsync();

        Task<Result> ProcessPlaidLinkAsync();
    }

    public class UserAccountService(
        IPlaidAdapter plaidAdapter,
        IPlaidUserAccountRepository plaidUserAccountRepository,
        IUserAccountRepository userAccountRepository,
        IUserSessionService userSessionService) : IUserAccountService
    {
        public async Task<Result> CreateUserAsync(string name, string email, string username)
        {
            var response = await plaidAdapter.CreateUserAsync();

            if (!response.IsSuccess)
            {
                return response;
            }

            DateTime now = DateTime.Now;

            var newUserId = Guid.NewGuid();

            var repoReponse = await userAccountRepository.AddUserAsync(new User
            {
                Id = newUserId,
                Name = name,
                Email = email,
                Username = username,
                CreatedAt = now,
                PlaidUser = new PlaidUser
                {
                    Id = response.Value!,
                    ClientUserId = PlaidConstants.ClientUserId,
                    CreatedAt = now,
                },
            });

            if (!repoReponse.IsSuccess)
            {
                return repoReponse;
            }

            userSessionService.SetCurrentUser(new UserAccount(newUserId, username, name, email));

            return Result.Success();
        }

        public async Task<Result<string>> CreateLinkAsync()
        {
            var plaidUser = await plaidUserAccountRepository.GetPlaidUserByIdAsync(userSessionService.CurrentUser!.Id);

            var response = await plaidAdapter.CreateLinkAsync(plaidUser?.Id ?? string.Empty);

            if (!response.IsSuccess)
            {
                return Result<string>.Failure(response.ErrorMessage, additionalInformation: response.AdditionalInformation);
            }

            var repoResponse  = await plaidUserAccountRepository.UpdatePlaidUserAsync(response.Value!.Id, response.Value.LinkToken);

            if (!repoResponse.IsSuccess)
            {
                return Result<string>.Failure(repoResponse.ErrorMessage);
            }

            return Result<string>.Success(response.Value!.HostedLinkUrl);
        }

        public async Task<Result> ProcessPlaidLinkAsync()
        {
            var plaidUser = await plaidUserAccountRepository.GetPlaidUserByIdAsync(userSessionService.CurrentUser!.Id);

            var response = await plaidAdapter.GetLinkSessionsAsync(plaidUser!.LinkToken!);

            if (!response.IsSuccess)
            {
                return Result.Failure(response.ErrorMessage, additionalInformation: response.AdditionalInformation);
            }

            // Maybe we can create some kind of callback that the view model will define to be ran after all of the accounts are finished being processed
            // Maybe can display a loading icon in the bottom right with a progress bar (or something like that?)
            // And a green checkmark alert letting the user know their info has been processed and they need to refresh.
            await ProcessLinkSessionsAsync(response!.Value!, plaidUser.Id);

            return Result.Success();
        }

        private async Task ProcessLinkSessionsAsync(List<PlaidLinkSession> linkSessions, string plaidUserId)
        {
            List<BudgetingApp.Infrastructure.Data.PlaidItem> listOfPlaidItems = [];

            foreach (var linkSession in linkSessions)
            {
                foreach (var addedItem in linkSession.AddedItems)
                {
                    var accessToken = await plaidAdapter.ExchangePublicTokenAsync(addedItem.PublicToken);

                    var now = DateTime.UtcNow;

                    var plaidItemToAdd = new PlaidItem
                    {
                        Id = Guid.NewGuid(),
                        AccessToken = accessToken,
                        InstitutionId = addedItem.Institution!.Id,
                        InstitutionName = addedItem.Institution.Name,
                        LastSyncedAt = now,
                        CreatedAt = now,
                        PlaidUserId = plaidUserId
                    };

                    var transactions = await plaidAdapter.SyncTransactions(accessToken);

                    foreach (var account in addedItem.Accounts)
                    {
                        plaidItemToAdd.PlaidAccounts.Add(new BudgetingApp.Infrastructure.Data.PlaidAccount
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            LastSyncedAt = now,
                            Mask = account.Mask,
                            Name = account.Name,
                            PlaidTransactions = [.. transactions
                            .Where(x => x.AccountId == account.Id)
                            .Select(x => new BudgetingApp.Infrastructure.Data.PlaidTransaction
                            {
                                Id = Guid.NewGuid(),
                                TransactionId = x.Id,
                                Amount = x.Amount,
                                AuthorizedDate = x.AuthorizedDate,
                                MerchantName = x.MerchantName,
                                Pending = x.Pending,
                                PlaidAccountId = account.Id
                            })]
                        });
                    }

                    listOfPlaidItems.Add(plaidItemToAdd);
                }
            }

            await plaidUserAccountRepository.AddPlaidItemsAsync(listOfPlaidItems);
        }
    }
}
