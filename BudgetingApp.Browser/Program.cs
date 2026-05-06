using Avalonia;
using Avalonia.Browser;
using BudgetingApp;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;

internal sealed partial class Program
{
    private static Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddBudgetingAppServices();
            })
            .Build();

        return BuildAvaloniaApp(host.Services)
            .WithInterFont()
#if DEBUG
            .WithDeveloperTools()
#endif
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp(IServiceProvider services)
    {
        return AppBuilder.Configure<App>(() =>
        {
            var app = new App();
            app.Initialize(services);
            return app;
        });
    }
}