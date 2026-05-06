using BudgetingApp.ViewModels;
using BudgetingApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBudgetingAppServices(this IServiceCollection services)
        {
            services.AddSingleton<AccountView>();
            services.AddSingleton<MainView>();

            services.AddSingleton<AccountViewModel>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>();

            return services;
        }
    }
}
