using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BudgetingApp.Data.Data.Enums;

namespace BudgetingApp.Data.Data;

public class PlaidAccount
{
    [Key]
    public Guid Id { get; init; }
    public string? AccountId { get; init; }
    public required string Name { get; init; }
    public decimal? CurrentBalance { get; init; }
    public decimal? BalanceAvailable { get; init; }
    public decimal BalanceLimit { get; init; }
    public bool IsHidden { get; init; }
    public DateTime LastSyncedAt { get; init; }
    public string? Mask { get; init; }
    public AccountType? Type { get; init; }
    public AccountSubType? SubType { get; init; }
    public string? VerificationStatus { get; init; }
    public Guid PlaidConnectionId { get; init; }

    [ForeignKey(nameof(PlaidConnectionId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PlaidConnection PlaidConnection { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public List<PlaidTransaction> PlaidTransactions { get; init; } = [];
}
