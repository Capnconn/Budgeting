using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetingApp.Infrastructure.Data
{
    public class UserSettings
    {
        [Key]
        public Guid Id { get; set; }
        public int BudgetPeriodStartDay { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public User UserData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
