using BudgetingApp.Common.Results;
using BudgetingApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Infrastructure.Repositories
{
    public interface IUserAccountRepository
    {
        Task<Result> AddUserAsync(User user);
    }

    public class UserAccountRepository(BudgetContext context) : IUserAccountRepository
    {
        public async Task<Result> AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
