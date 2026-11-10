using System;
using System.Collections.Generic;
using System.Text;
using BudgetingApp.Data.Data.Enums;

namespace BudgetingApp.Services.Data;

public record Transaction(
    string? MerchantName,
    decimal? Amount,
    string? Category,
    DateOnly? AuthorizedDate,
    bool? Pending)
{
}
