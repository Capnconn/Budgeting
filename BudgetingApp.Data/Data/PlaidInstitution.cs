using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetingApp.Data.Data;

public class PlaidInstitution
{
    public string? InstitutionId { get; init; }

    public string? InstitutionName { get; init; }

    public Guid PlaidConnectionId { get; init; }

    [ForeignKey(nameof(PlaidConnectionId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PlaidConnection PlaidConnection { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
