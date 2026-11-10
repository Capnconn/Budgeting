using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Data.Data;

public class PlaidConnection
{
    [Key]
    public Guid Id { get; init; }
    public string AccessToken { get; init; } = string.Empty;

    [Required]
    public string PlaidConnectionId { get; init; } = string.Empty;

    /// <summary>
    /// Cursor used when additional requests are required for fetching transactions.
    /// Use this value as the "Cursor" argument in the request.
    /// </summary>
    public string? NextCursor { get; init; }

    /// <summary>
    /// If true, then there are additional transactions, can be fetched using the 
    /// "NextCursor" property in subsequent requests for transactions.
    /// </summary>
    public bool? HasMore { get; init; }
    public string ConnectionStatus { get; init; } = string.Empty;
    public string? ErrorCode { get; init; } = null;
    public DateTimeOffset? ConsentExpirationTime { get; init; }
    public DateTime LastSyncedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string PlaidUserId { get; init; } = string.Empty;

    public List<PlaidAccount> PlaidAccounts { get; init; } = [];

    public PlaidInstitution? PlaidInstitution { get; init; }

    [ForeignKey(nameof(PlaidUserId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PlaidUser PlaidUser { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [NotMapped]
    public int NumberOfAccounts => PlaidAccounts.Count;
}
