using BudgetingApp.Services;
using BudgetingApp.ViewModels;
using BudgetingApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        services.AddSingleton<MainView>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        return services;
    }
}
