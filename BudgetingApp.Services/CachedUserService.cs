using BudgetingApp.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Services;

public static class CachedUserService
{
    public static CachedUser CachedUser { get; private set; } = new CachedUser(Guid.Empty, string.Empty, string.Empty, string.Empty);

    public static void SetCachedUser(CachedUser user)
    {
        CachedUser = user;
    }
}
