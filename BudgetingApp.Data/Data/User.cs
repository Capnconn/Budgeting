using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Data.Data;

public class User
{
    [Key]
    public Guid Id { get; init; }          // surrogate key is safer than username
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }  // nullable — not deleted by default

    public PlaidUser? PlaidUser { get; init; }
    public UserSettings? UserSettings { get; init; }
}
