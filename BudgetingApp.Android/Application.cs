using Android.App;
using Android.Runtime;
using Avalonia;
using Avalonia.Android;
using Microsoft.Extensions.Hosting;

namespace BudgetingApp.Android
{
    [Application]
    public class Application : AvaloniaAndroidApplication<App>
    {
        protected Application(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

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
