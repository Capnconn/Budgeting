using System;
using System.Collections.Generic;
using System.Text;
using BudgetingApp.Data.Data.Enums;

namespace BudgetingApp.Services.Data;

public record PlaidAccount(
    string Name,
    string Mask,
    AccountType? Type,
    decimal CurrentBalance,
    bool IsHidden,
    DateOnly LastSyncedAt)
{
}
