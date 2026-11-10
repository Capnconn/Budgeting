using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetingApp.Data.Data;

// Used only by `dotnet ef` at design time (scaffolding migrations). The real app configures
// BudgetContext via DI in ServiceCollectionExtensions.AddDataServices instead - the connection
// string here never needs to point at a real database, it's just enough for EF to build the model.
public class BudgetContextFactory : IDesignTimeDbContextFactory<BudgetContext>
{
    public BudgetContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BudgetContext>();
        optionsBuilder.UseSqlite("Data Source=design-time.db");

        return new BudgetContext(optionsBuilder.Options);
    }
}
