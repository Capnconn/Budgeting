using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using BudgetingApp.Data.Data;
using BudgetingApp.ViewModels;
using BudgetingApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BudgetingApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Default logic doesn't auto detect windows theme anymore in designer
        // to stop light mode, force here
        if (Design.IsDesignMode)
        {
            RequestedThemeVariant = ThemeVariant.Dark;
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // The below line of code is in the documentation but does not work?
        // BindingPlugins.DataValidators.RemoveAt(0);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            // Git-ignored overrides: Plaid keys and machine-specific paths live here, not in appsettings.json.
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .Build();

        var collection = new ServiceCollection();
        collection.AddViewServices(configuration);

        var services = collection.BuildServiceProvider();

        using (var scope = services.CreateScope())
        {
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BudgetContext>>();
            var context = contextFactory.CreateDbContext();

            context.Database.Migrate();
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
        {
            singleViewFactoryApplicationLifetime.MainViewFactory = () => new MainView
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
