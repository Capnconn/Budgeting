using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Adapter.Data;

public class PlaidTransaction
{
    public string? Id { get; set; } = string.Empty;
    public string? AccountId { get; set; } = string.Empty;
    public string? MerchantName { get; set; }
    public decimal? Amount { get; set; }
    public DateOnly? AuthorizedDate { get; set; }
    public bool? Pending { get; set; }        
}
