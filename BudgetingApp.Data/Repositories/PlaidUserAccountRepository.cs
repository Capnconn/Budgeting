using BudgetingApp.Common.Results;
using BudgetingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Infrastructure.Repositories
{
    // TODO: Remove usage of Result.
    public interface IPlaidUserAccountRepository
    {
        Task AddPlaidItemsAsync(List<PlaidItem> plaidItems);
        Task<PlaidUser?> GetPlaidUserByIdAsync(Guid id);

        Task<Result> UpdatePlaidUserAsync(string id, string linkToken);
    }

    public class PlaidUserAccountRepository(BudgetContext context) : IPlaidUserAccountRepository
    {
        public async Task AddPlaidItemsAsync(List<PlaidItem> plaidItems)
        {
            await context.PlaidItems.AddRangeAsync(plaidItems);
            await context.SaveChangesAsync();
        }

        public async Task<PlaidUser?> GetPlaidUserByIdAsync(Guid id) => await context.PlaidUsers.FirstOrDefaultAsync(x => x.UserId == id);

        public async Task<Result> UpdatePlaidUserAsync(string id, string linkToken)
        {
            var userToUpdate = await context.PlaidUsers.FirstOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());

            if (userToUpdate is null)
            {
                return Result.Failure($"Unable to find plaid user with the id: {id}.");
            }

            userToUpdate.LinkToken = linkToken;
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
