using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetingApp.Infrastructure.Data
{
    public class PlaidUser
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string ClientUserId { get; set; } = string.Empty;
        public string? LinkToken { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public User User { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public List<PlaidItem> PlaidItems { get; set; } = [];
    }
}
