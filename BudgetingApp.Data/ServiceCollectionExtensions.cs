using BudgetingApp.Infrastructure.Configuration;
using BudgetingApp.Infrastructure.Data;
using BudgetingApp.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration
            services.AddSingleton(configuration.GetSection(DatabaseOptions.OptionsName).Get<DatabaseOptions>()!);

            // Context
            services.AddTransient<BudgetContext>();

            // Repositories
            services.AddTransient<IPlaidUserAccountRepository, PlaidUserAccountRepository>();
            services.AddTransient<IUserAccountRepository, UserAccountRepository>();

            return services;
        }
    }
}
