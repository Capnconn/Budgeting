using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Data.Data;

public class UserSettings
{
    [Key]
    public Guid Id { get; init; }
    public int BudgetPeriodStartDay { get; init; }
    public Guid UserId { get; init; }
    [ForeignKey(nameof(UserId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public User UserData { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
