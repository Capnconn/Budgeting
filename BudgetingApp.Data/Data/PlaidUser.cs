using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetingApp.Data.Data;

public class PlaidUser
{
    [Key]
    public string Id { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string ClientUserId { get; init; } = string.Empty;
    public string? LinkToken { get; set; }
    public DateTime CreatedAt { get; init; }

    [ForeignKey(nameof(UserId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public User User { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public List<PlaidConnection> PlaidItems { get; init; } = [];
}
