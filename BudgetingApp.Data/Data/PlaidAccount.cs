using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Infrastructure.Data
{
    public class PlaidAccount
    {
        [Key]
        public Guid Id { get; set; }
        public string? AccountId { get; set; }
        public string? Name { get; set; }
        public decimal? CurrentBalance { get; set; }
        public Decimal BalanceAvailable { get; set; }
        public decimal BalanceLimit { get; set; }
        public bool IsHidden { get; set; }
        public DateTime LastSyncedAt { get; set; }
        public string? Mask { get; set; }
        public string? Type { get; set; }
        public string? VerificationStatus { get; set; }
        public Guid PlaidItemId { get; set; }
        [ForeignKey(nameof(PlaidItemId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PlaidItem PlaidItem { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [ForeignKey(nameof(AccountId))]
        public List<PlaidTransaction> PlaidTransactions { get; set; } = [];
    }
}
