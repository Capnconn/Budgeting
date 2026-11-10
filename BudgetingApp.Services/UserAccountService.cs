using System.ComponentModel;
using BudgetingApp.Adapter.Adapters;
using BudgetingApp.Adapter.Constants;
using BudgetingApp.Common.Results;
using BudgetingApp.Data.Data;
using BudgetingApp.Data.Data.Enums;
using BudgetingApp.Data.Repositories;
using BudgetingApp.Services.Data;
using Going.Plaid.Entity;
using AccountType = BudgetingApp.Data.Data.Enums.AccountType;
using PlaidAccount = BudgetingApp.Services.Data.PlaidAccount;
using Transaction = BudgetingApp.Services.Data.Transaction;

namespace BudgetingApp.Services;

public class UserAccountService(
    PlaidAdapter plaidAdapter,
    PlaidUserAccountRepository plaidUserAccountRepository,
    UserAccountRepository userAccountRepository)
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

        var repoResponse = await userAccountRepository.AddUserAsync(new User
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

        if (!repoResponse.IsSuccess)
        {
            return repoResponse;
        }

        CachedUserService.SetCachedUser(new CachedUser(newUserId, username, name, email));

        return Result.Success();
    }

    public async Task<Result<string>> CreateLinkAsync()
    {
        var plaidUser = await plaidUserAccountRepository.GetPlaidUserByIdAsync(CachedUserService.CachedUser.Id);

        if (plaidUser is null)
        {
            return Result<string>.Failure("No Plaid user found for the current user; cannot create a Link token.");
        }

        var response = await plaidAdapter.CreateLinkAsync(plaidUser.Id);

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

    public async Task<List<PlaidAccount>> GetAccountsAsync()
    {
        var plaidAccounts = await plaidUserAccountRepository.GetPlaidAccountsAsync();

        return plaidAccounts.Select(x => new PlaidAccount(x.Name, x.Mask ?? "None", x.Type, x.CurrentBalance ?? -1, x.IsHidden, DateOnly.FromDateTime(x.LastSyncedAt))).ToList();
    }

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        var plaidTransactions = await plaidUserAccountRepository.GetPlaidTransactionsAsync();

        return [.. plaidTransactions.Select(x => new Transaction(x.MerchantName, x.Amount, x.Category, x.AuthorizedDate, x.Pending))];
    }

    public async Task LinkNewConnectionAsync(string publicToken)
    {
        var accessToken = await plaidAdapter.ExchangePublicTokenAsync(publicToken);
        var item = await plaidAdapter.SyncItemAsync(accessToken);
        var accounts = await plaidAdapter.SyncAccountsAsync(accessToken);
        var transactions = await plaidAdapter.SyncTransactionsAsync(accessToken);

        // Runs from BudgetingApp.Server's Wolverine handler, a separate process from the Desktop app,
        // so CachedUserService's static state (populated by the Desktop app's own process) isn't
        // available here. This is a single-local-user app, so look the user up directly instead.
        var user = await userAccountRepository.GetFirstUserAsync();
        var plaidUser = user is not null ? await plaidUserAccountRepository.GetPlaidUserByIdAsync(user.Id) : null;
        var now = DateTime.Now;

        var connection = new PlaidConnection
        {
            Id = Guid.NewGuid(),
            AccessToken = accessToken,
            PlaidConnectionId = item.ItemId!,
            ConsentExpirationTime = item.ConsentExpirationTime,
            ConnectionStatus = item.Error is null ? "Active" : "Error",
            ErrorCode = item.Error?.ErrorCode,
            CreatedAt = now,
            UpdatedAt = now,
            LastSyncedAt = now,
            PlaidUserId = plaidUser?.Id ?? string.Empty,
            PlaidInstitution = new PlaidInstitution
            {
                InstitutionId = item.InstitutionId,
                InstitutionName = item.InstitutionName
            },
            PlaidAccounts = [.. accounts.Select(a => new BudgetingApp.Data.Data.PlaidAccount
            {
                Id = Guid.NewGuid(),
                AccountId = a.AccountId,
                Name = a.Name,
                Mask = a.Mask,
                CurrentBalance = a.Balances.Current,
                BalanceAvailable = a.Balances.Available,
                Type = Enum.TryParse(a.Type.ToString(), true, out AccountType type) ? type : AccountType.Unknown,
                SubType = Enum.TryParse(a.Subtype?.ToString(), true, out AccountSubType subType) ? subType : AccountSubType.Unknown,
                VerificationStatus = a.VerificationStatus?.ToString(),
                IsHidden = false,
                LastSyncedAt = now,
                PlaidTransactions = [.. transactions
                    .Where(t => t.AccountId == a.AccountId)
                    .Select(t => new PlaidTransaction
                    {
                        Id = Guid.NewGuid(),
                        TransactionId = t.TransactionId,
                        PlaidAccountId = a.AccountId,
                        Amount = t.Amount,
                        AuthorizedDate = t.AuthorizedDate,
                        Category = t.PersonalFinanceCategory?.Detailed ?? t.PersonalFinanceCategory?.Primary,
                        MerchantName = t.MerchantName,
                        Pending = t.Pending,
                        PendingTransactionId = t.PendingTransactionId
                    })]
            })]
        };

        await plaidUserAccountRepository.AddPlaidConnectionAsync(connection);
    }

    /// <summary>
    /// Attempts to bind to the first user, if one exists.
    /// </summary>
    /// <returns>The bound user OR a new user if none was found.</returns>
    public async Task<CachedUser> TryToBindToUserAsync()
    {
        var firstUser = await userAccountRepository.GetFirstUserAsync();

        CachedUser cachedUser;
        if (firstUser is not null)
        {
            cachedUser = new CachedUser(firstUser.Id, firstUser.Username, firstUser.Name, firstUser.Email);
            CachedUserService.SetCachedUser(cachedUser);
            return cachedUser;
        }

        var userId = await plaidAdapter.CreateUserAsync();

        User newUser = new()
        {
            CreatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            Email = "test@gmail.com",
            Name = "Connor",
            PlaidUser = new PlaidUser
            {
                Id = userId.Value!,
                ClientUserId = PlaidConstants.ClientUserId,
                CreatedAt = DateTime.Now
            },
            Username = "Capnconn",
        };

        await userAccountRepository.AddUserAsync(newUser);

        cachedUser = new CachedUser(newUser.Id, newUser.Username, newUser.Name, newUser.Email);
        CachedUserService.SetCachedUser(cachedUser);

        return cachedUser;
    }
}
