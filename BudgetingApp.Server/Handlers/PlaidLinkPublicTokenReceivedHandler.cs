using BudgetingApp.Server.Messages;
using BudgetingApp.Services;
using System.IO.Pipes;

namespace BudgetingApp.Server.Handlers;

public static class PlaidLinkPublicTokenReceivedHandler
{
    // Must match BudgetingApp/ViewModels/MainViewModel.LinkFinishedPipeName.
    private const string LinkFinishedPipeName = "BudgetingApp.PlaidLinkFinished";

    public static async Task Handle(PlaidLinkPublicTokenReceived message, UserAccountService userAccountService)
    {
        await userAccountService.LinkNewConnectionAsync(message.PublicToken);
        await NotifyDesktopAsync();
    }

    // Best-effort ping so the Desktop app can refresh instead of polling the DB. If the desktop
    // app isn't running/listening, this just times out - the data is already saved either way.
    private static async Task NotifyDesktopAsync()
    {
        using var pipeClient = new NamedPipeClientStream(".", LinkFinishedPipeName, PipeDirection.Out, PipeOptions.Asynchronous);
        using var connectCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        try
        {
            await pipeClient.ConnectAsync(connectCts.Token);
        }
        catch (OperationCanceledException)
        {
        }
    }
}
