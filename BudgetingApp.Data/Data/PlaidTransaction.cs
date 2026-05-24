using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetingApp.Infrastructure.Data
{
    public class PlaidTransaction
    {
        public Guid Id { get; set; }
        public string? TransactionId { get; set; }
        public string? MerchantName { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? AuthorizedDate { get; set; }
        public bool? Pending { get; set; }
        public string? PlaidAccountId { get; set; }
        [ForeignKey(nameof(PlaidAccountId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PlaidAccount PlaidAccount { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
