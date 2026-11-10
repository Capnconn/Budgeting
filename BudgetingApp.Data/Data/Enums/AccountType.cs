namespace BudgetingApp.Data.Data.Enums;

/// <summary>
/// These values are actually Plaid's defined account <c>subtype</c> values, not Plaid's <c>type</c> values.
/// See https://plaid.com/docs/api/accounts/#account-type-schema
/// </summary>
public enum AccountType
{
    Unknown,
    Credit,
    Depository,
    Investment,
    Loan,
    Other
}
