using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BudgetingApp.ViewModels;
using BudgetingApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BudgetingApp
{
    public partial class App : Application
    {
        private IServiceProvider? _services;

        public void Initialize(IServiceProvider services)
        {
            _services = services;
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = _services!.GetRequiredService<MainWindow>();
            }
            else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
            {
                singleViewFactoryApplicationLifetime.MainViewFactory = () => _services!.GetRequiredService<MainView>();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = _services!.GetRequiredService<MainView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}