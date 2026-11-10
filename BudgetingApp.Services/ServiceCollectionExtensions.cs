using BudgetingApp.Adapter;
using BudgetingApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAdapterServices(configuration);
        services.AddDataServices(configuration);

        services.AddSingleton<UserAccountService>();

        return services;
    }
}
