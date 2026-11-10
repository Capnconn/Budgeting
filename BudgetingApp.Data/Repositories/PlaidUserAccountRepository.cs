using BudgetingApp.Common.Results;
using BudgetingApp.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Data.Repositories;

// TODO: Remove usage of Result.
public class PlaidUserAccountRepository(IDbContextFactory<BudgetContext> contextFactory)
{
    public async Task AddPlaidAccountAsync(List<PlaidAccount> plaidAccounts)
    {
        var context = await contextFactory.CreateDbContextAsync();
        await context.PlaidAccounts.AddRangeAsync(plaidAccounts);
        await context.SaveChangesAsync();
    }

    public async Task AddPlaidConnectionAsync(PlaidConnection plaidConnections)
    {
        var context = await contextFactory.CreateDbContextAsync();
        await context.PlaidConnections.AddAsync(plaidConnections);
        await context.SaveChangesAsync();
    }

    public async Task AddPlaidConnectionsAsync(List<PlaidConnection> plaidConnections)
    {
        var context = await contextFactory.CreateDbContextAsync();
        await context.PlaidConnections.AddRangeAsync(plaidConnections);
        await context.SaveChangesAsync();
    }

    public async Task AddPlaidTransactionsAsync(List<PlaidTransaction> plaidTransactions)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var accountIds = context.PlaidAccounts.Select(x => x.Id).ToHashSet();

        foreach (PlaidTransaction plaidTransaction in plaidTransactions)
        {
            if (!accountIds.TryGetValue(plaidTransaction.Id, out _))
            {
                // log something here about there not being a matching account in the database, since this would be weird.
                continue;
            }

            await context.PlaidTransactions.AddAsync(plaidTransaction);
        }
    }

    public async Task<List<PlaidAccount>> GetPlaidAccountsAsync()
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.PlaidAccounts.Include(x => x.PlaidConnection).AsNoTracking().ToListAsync();
    }

    public async Task<List<PlaidTransaction>> GetPlaidTransactionsAsync()
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.PlaidTransactions.AsNoTracking().ToListAsync();
    }

    public async Task<PlaidUser?> GetPlaidUserByIdAsync(Guid id)
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.PlaidUsers.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<Result> UpdatePlaidUserAsync(string id, string linkToken)
    {
        var context = await contextFactory.CreateDbContextAsync();
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
