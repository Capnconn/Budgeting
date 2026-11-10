using BudgetingApp.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BudgetingApp.Data.Data;

public class BudgetContext(DbContextOptions<BudgetContext> options) : DbContext(options)
{
    public DbSet<PlaidAccount> PlaidAccounts { get; set; }
    public DbSet<PlaidConnection> PlaidConnections { get; set; }
    public DbSet<PlaidInstitution> PlaidInstitutions { get; set; }
    public DbSet<PlaidTransaction> PlaidTransactions { get; set; }
    public DbSet<PlaidUser> PlaidUsers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaidAccount>()
            .Property(e => e.Type)
            .HasConversion<string>();

        modelBuilder.Entity<PlaidAccount>()
            .Property(e => e.SubType)
            .HasConversion<string>();

        // PlaidAccount <-> PlaidTransaction: transactions are matched to accounts by Plaid's own
        // account_id, not our surrogate key, since that's the identifier Plaid gives us up front.
        modelBuilder.Entity<PlaidTransaction>()
            .HasOne(t => t.PlaidAccount)
            .WithMany(a => a.PlaidTransactions)
            .HasForeignKey(t => t.PlaidAccountId)
            .HasPrincipalKey(a => a.AccountId);

        // PlaidConnection <-> PlaidAccount
        modelBuilder.Entity<PlaidAccount>()
            .HasOne(a => a.PlaidConnection)
            .WithMany(c => c.PlaidAccounts)
            .HasForeignKey(a => a.PlaidConnectionId);

        // PlaidConnection <-> PlaidInstitution: shared-key 1:1, PlaidInstitution has no surrogate key
        // of its own - its identity IS the connection it describes.
        modelBuilder.Entity<PlaidInstitution>()
            .HasKey(institution => institution.PlaidConnectionId);

        modelBuilder.Entity<PlaidConnection>()
            .HasOne(c => c.PlaidInstitution)
            .WithOne(institution => institution.PlaidConnection)
            .HasForeignKey<PlaidInstitution>(institution => institution.PlaidConnectionId);

        // PlaidUser <-> PlaidConnection
        modelBuilder.Entity<PlaidConnection>()
            .HasOne(c => c.PlaidUser)
            .WithMany(u => u.PlaidItems)
            .HasForeignKey(c => c.PlaidUserId);

        // User <-> PlaidUser: 1:1
        modelBuilder.Entity<PlaidUser>()
            .HasOne(pu => pu.User)
            .WithOne(u => u.PlaidUser)
            .HasForeignKey<PlaidUser>(pu => pu.UserId);

        // User <-> UserSettings: 1:1
        modelBuilder.Entity<UserSettings>()
            .HasOne(s => s.UserData)
            .WithOne(u => u.UserSettings)
            .HasForeignKey<UserSettings>(s => s.UserId);
    }
}
