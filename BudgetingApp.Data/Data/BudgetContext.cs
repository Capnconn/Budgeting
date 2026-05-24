using BudgetingApp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace BudgetingApp.Infrastructure.Data
{
    public class BudgetContext : DbContext
    {
        private readonly string _databasePath;

        public DbSet<PlaidAccount> PlaidAccounts { get; set; }
        public DbSet<PlaidItem> PlaidItems { get; set; }
        public DbSet<PlaidTransaction> PlaidTransactions { get; set; }
        public DbSet<PlaidUser> PlaidUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        public BudgetContext(DatabaseOptions options)
        {
            _databasePath = options.DatabaseFullName;

            if (!File.Exists(_databasePath))
            {
                File.Create(_databasePath).Close();
            }

            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaidTransaction>()
                .HasOne(t => t.PlaidAccount)
                .WithMany(a => a.PlaidTransactions)
                .HasForeignKey(t => t.PlaidAccountId)
                .HasPrincipalKey(a => a.AccountId);
        }
    }
}
