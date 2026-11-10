using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BudgetingApp.Data.Data.Enums;

namespace BudgetingApp.Data.Data;

public class PlaidTransaction
{
    public Guid Id { get; init; }
    public string? TransactionId { get; init; }
    public string? MerchantName { get; init; }
    public decimal? Amount { get; init; }
    public DateOnly? AuthorizedDate { get; init; }
    public string? Category { get; init; }
    public bool? Pending { get; init; }
    public string? PendingTransactionId { get; init; }
    public string? PlaidAccountId { get; init; }
    [ForeignKey(nameof(PlaidAccountId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PlaidAccount PlaidAccount { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
