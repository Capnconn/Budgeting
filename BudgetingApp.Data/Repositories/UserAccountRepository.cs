using BudgetingApp.Common.Results;
using BudgetingApp.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Data.Repositories;
public class UserAccountRepository(IDbContextFactory<BudgetContext> contextFactory)
{
    public async Task<Result> AddUserAsync(User user)
    {
        var context = await contextFactory.CreateDbContextAsync();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<User?> GetFirstUserAsync()
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.Users.FirstOrDefaultAsync();
    }
}
