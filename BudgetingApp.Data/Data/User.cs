using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Infrastructure.Data
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }          // surrogate key is safer than username
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }  // nullable — not deleted by default

        public PlaidUser? PlaidUser { get; set; }
        public UserSettings? UserSettings { get; set; }
    }
}
