using Budget.Adapters;
using Going.Plaid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Adapter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAdapterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Going.Plaid does not expect all of the configuration, it only accepts the Plaid config.
            services.AddPlaid(configuration.GetSection(PlaidOptions.SectionKey));
            services.AddSingleton<IPlaidAdapter, PlaidAdapter>();

            return services;
        }
    }
}
