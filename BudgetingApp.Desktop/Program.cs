using Avalonia;
using Microsoft.Extensions.Hosting;
using System;

namespace BudgetingApp.Desktop
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddBudgetingAppServices();
                })
                .Build();

            BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp(IServiceProvider services)
            => AppBuilder.Configure<App>(() =>
            {
                var app = new App();
                app.Initialize(services);
                return app;   
            })
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
    }
}
