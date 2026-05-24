using BudgetingApp.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Services
{
    public interface IUserSessionService
    {
        UserAccount? CurrentUser { get; }
        void SetCurrentUser(UserAccount user);
        void Clear();
    }

    public class UserSessionService : IUserSessionService
    {
        public UserAccount? CurrentUser { get; private set; }

        public void SetCurrentUser(UserAccount user)
        {
            CurrentUser = user;
        }

        public void Clear() => CurrentUser = null;
    }
}
