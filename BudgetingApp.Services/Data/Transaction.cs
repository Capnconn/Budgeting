using BudgetingApp.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Services.Data
{
    public class Transaction
    {
        public string? MerchantName { get; set; }
        public decimal? Amount { get; set; }
        public TransactionCategory Category { get; set; }
        public DateOnly? AuthorizedDate { get; set; }
        public bool? Pending { get; set; }
    }
}
