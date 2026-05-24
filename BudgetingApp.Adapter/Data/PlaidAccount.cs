using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Adapter.Data
{
    public class PlaidAccount
    {
        public string Id { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mask { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string SubType { get; set; } = string.Empty;
        public string VerificationStatus { get; set; } = string.Empty;
    }
}
