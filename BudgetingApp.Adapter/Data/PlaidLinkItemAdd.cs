using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Adapter.Data
{
    public class PlaidLinkItemAdd
    {
        public string PublicToken { get; set; } = string.Empty;

        public List<PlaidAccount> Accounts { get; set; } = [];
        public PlaidInstitution? Institution { get; set; }
    }
}
