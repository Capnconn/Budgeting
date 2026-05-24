using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Infrastructure.Data
{
    public class PlaidItem
    {
        [Key]
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        public string PlaidItemId { get; set; } = string.Empty;
        public string? InstitutionId { get; set; }
        public string? InstitutionName { get; set; }

        /// <summary>
        /// Cursor used when additional requests are required for fetching transactions.
        /// Use this value as the "Cursor" argument in the request.
        /// </summary>
        public string? NextCursor { get; set; }

        /// <summary>
        /// If true, then there are additional transactions, can be fetched using the 
        /// "NextCursor" property in subsequent requests for transactions.
        /// </summary>
        public bool? HasMore { get; set; }
        public string ItemStatus { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public DateTime ConsentExpirationTime { get; set; }
        public DateTime LastSyncedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PlaidUserId { get; set; } = string.Empty;

        [ForeignKey(nameof(PlaidUserId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PlaidUser PlaidUser { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public List<PlaidAccount> PlaidAccounts { get; set; } = [];

        [NotMapped]
        public int NumberOfAccounts => PlaidAccounts.Count;
    }
}
