using BudgetingApp.Adapter.Constants;
using BudgetingApp.Adapter.Data;
using BudgetingApp.Common.Results;
using Going.Plaid;
using Going.Plaid.Accounts;
using Going.Plaid.Entity;
using Going.Plaid.Item;
using Going.Plaid.Link;
using Going.Plaid.Transactions;
using Going.Plaid.User;

namespace Budget.Adapters
{
    // TODO: Remove usage of result.
    public interface IPlaidAdapter
    {
        Task<Result<string>> CreateUserAsync();

        Task<Result<PlaidLink>> CreateLinkAsync(string id);

        Task<string> ExchangePublicTokenAsync(string publicToken);

        Task<Result<List<PlaidLinkSession>>> GetLinkSessionsAsync(string linkToken);

        Task<List<PlaidTransaction>> SyncTransactions(string accessToken);
    }

    public class PlaidAdapter(PlaidClient plaidClient) : IPlaidAdapter
    {
        public async Task<Result<string>> CreateUserAsync()
        {
            var response = await plaidClient.UserCreateAsync(new UserCreateRequest
            {
                ClientUserId = PlaidConstants.ClientUserId
            });

            if (!response.IsSuccessStatusCode)
            {
                return Result<String>.Failure(response.Error!.ErrorCode, additionalInformation: response.Error.SuggestedAction);
            }

            return Result<string>.Success(response.UserId!);
        }

        public async Task<Result<PlaidLink>> CreateLinkAsync(string id)
        {
            var response = await plaidClient.LinkTokenCreateAsync(
                new LinkTokenCreateRequest
                {
                    User = new LinkTokenCreateRequestUser
                    {
                        ClientUserId = PlaidConstants.ClientUserId
                    },
                    UserId = id,
                    ClientName = "BudgetApp",
                    Products = [Products.Transactions],
                    CountryCodes = [CountryCode.Us],
                    Language = Language.English,
                    HostedLink = new LinkTokenCreateHostedLink
                    {
                        CompletionRedirectUri = "https://budget.local/"
                    },
                    EnableMultiItemLink = true
                });

            if (!response.IsSuccessStatusCode)
            {
                return Result<PlaidLink>.Failure(response.Error!.ErrorMessage, additionalInformation: response.Error.SuggestedAction);
            }

            return Result<PlaidLink>.Success(new PlaidLink(response.UserId!, response.LinkToken, response.HostedLinkUrl!));
        }

        public async Task<Result<List<PlaidLinkSession>>> GetLinkSessionsAsync(string linkToken)
        {
            var response = await plaidClient.LinkTokenGetAsync(
                new LinkTokenGetRequest
                {
                    LinkToken = linkToken
                });

            if (!response.IsSuccessStatusCode)
            {
                return Result<List<PlaidLinkSession>>.Failure(response.Error!.ErrorMessage, additionalInformation: response.Error.SuggestedAction);
            }

            var linkSessions = response.LinkSessions is null
                ? []
                : response.LinkSessions!.Select(linkSessiont => new PlaidLinkSession
                {
                    AddedItems = [.. linkSessiont.Results!.ItemAddResults.Select(addedItem => new PlaidLinkItemAdd
                    {
                        PublicToken = addedItem.PublicToken,
                        Institution = new PlaidInstitution
                        {
                            Id = addedItem.Institution!.InstitutionId!,
                            Name = addedItem.Institution.Name!
                        },
                        Accounts = [.. addedItem.Accounts.Select(account => new PlaidAccount
                        {
                            Id = account.Id!,
                            Name = account.Name!,
                            Mask = account.Mask!,
                            Type = account.Type!,
                            SubType = account.Subtype!,
                            VerificationStatus = account.VerificationStatus!
                        })]
                    })]
                }).ToList();

            return Result<List<PlaidLinkSession>>.Success(linkSessions);
        }

        public async Task<string> ExchangePublicTokenAsync(string publicToken)
        {
            var response = await plaidClient.ItemPublicTokenExchangeAsync(new ItemPublicTokenExchangeRequest
            {
                PublicToken = publicToken
            });

            CheckIfSuccessful(response);

            return response.AccessToken;
        }


        public async Task<List<PlaidTransaction>> SyncTransactions(string accessToken)
        {
            var transactionsSyncRequest = new TransactionsSyncRequest
            {
                AccessToken = accessToken
            };

            List<PlaidTransaction> plaidTransactions = [];

            TransactionsSyncResponse response;

            do
            {
                response = await plaidClient.TransactionsSyncAsync(transactionsSyncRequest);

                CheckIfSuccessful(response);

                foreach (var added in response.Added)
                {
                    plaidTransactions.Add(new PlaidTransaction
                    {
                        Id = added.TransactionId,
                        AccountId = added.AccountId,
                        Amount = added.Amount,
                        AuthorizedDate = added.AuthorizedDate,
                        MerchantName = added.MerchantName,
                        Pending = added.Pending
                    });
                }

                transactionsSyncRequest.Cursor = response.NextCursor;
            }
            while (response.HasMore);

            return plaidTransactions;
        }

        //public async Task<string> Exchange(string publicToken)
        //{
        //    var response = await plaidClient.ItemPublicTokenExchangeAsync(
        //        new ItemPublicTokenExchangeRequest
        //        {
        //            PublicToken = publicToken
        //        });

        //    CheckIfSuccessful(response);

        //    return response.AccessToken;
        //}

        //public async Task<decimal?> GetAllBalances(List<string> accessTokens)
        //{
        //    decimal newBalance = 0;

        //    foreach (var accessToken in accessTokens)
        //    {
        //        var response = await plaidClient.AccountsBalanceGetAsync(
        //            new AccountsBalanceGetRequest
        //            {
        //                AccessToken = accessToken,
        //            });

        //        CheckIfSuccessful(response);

        //        newBalance += response.Accounts.Where(x => x.Balances.Current is not null).Sum(x => x.Balances.Current!.Value);
        //    }

        //    return newBalance;
        //}

        //public async Task<Dictionary<string, decimal?>> GetBalance(string accessToken)
        //{
        //    Dictionary<string, decimal?> balances = [];

        //    var response = await plaidClient.AccountsBalanceGetAsync(
        //        new AccountsBalanceGetRequest
        //        {
        //            AccessToken = accessToken,
        //        });

        //    CheckIfSuccessful(response);

        //    return response.Accounts.ToDictionary(k => k.AccountId, v => v.Balances.Current);
        //}

        //public async Task<IReadOnlyCollection<LinkTokenGetSessionsResponse>> GetLink(string linkToken)
        //{
        //    var response = await plaidClient.LinkTokenGetAsync(
        //        new LinkTokenGetRequest
        //        {
        //            LinkToken = linkToken
        //        });

        //    CheckIfSuccessful(response);

        //    return response.LinkSessions!;
        //}

        private static void CheckIfSuccessful(ResponseBase response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Error!.ToString());
            }
        }
    }
}
