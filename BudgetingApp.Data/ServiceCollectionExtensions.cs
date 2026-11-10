using BudgetingApp.Data.Configuration;
using BudgetingApp.Data.Data;
using BudgetingApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        var databaseOptions = configuration.GetSection(DatabaseOptions.OptionsName).Get<DatabaseOptions>()!;

        if (!File.Exists(databaseOptions.DatabaseFullName))
        {
            File.Create(databaseOptions.DatabaseFullName!).Close();
        }

        // Context
        services.AddPooledDbContextFactory<BudgetContext>(options => options.UseSqlite($"Data Source={databaseOptions.DatabaseFullName}"));

        // Repositories
        services.AddSingleton<PlaidUserAccountRepository>();
        services.AddSingleton<UserAccountRepository>();

        return services;
    }
}
