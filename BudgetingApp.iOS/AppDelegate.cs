using Avalonia;
using Avalonia.Controls;
using Avalonia.iOS;
using Avalonia.Media;
using Foundation;
using Microsoft.Extensions.Hosting;
using UIKit;

namespace BudgetingApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public partial class AppDelegate : AvaloniaAppDelegate<App>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            builder.AfterSetup((appBuilder) =>
            {
                var host = Host.CreateDefaultBuilder()
                 .ConfigureServices((context, services) =>
                 {
                     services.AddBudgetingAppServices();
                 })
                 .Build();

                var instance = appBuilder.Instance as BudgetingApp.App;
                instance!.Initialize(host.Services);
            });
            return base.CustomizeAppBuilder(builder)
                .WithInterFont();
        }
    }
}
