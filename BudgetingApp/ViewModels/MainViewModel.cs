using Avalonia.Controls;
using BudgetingApp.Enums;
using BudgetingApp.Services;
using BudgetingApp.Services.Data;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BudgetingApp.Pages;

namespace BudgetingApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly UserAccountService userAccountService;

    // TODO: Find a better way to supply the PublicBaseUrl (the URL plaid redirects after the plaid flow has been completed.
    private const string PublicBaseUrl = "https://semifinal-playing-unwarlike.ngrok-free.dev";
    private const AppPage StartingPage = AppPage.Account;

    // Must match BudgetingApp.Server's Handlers/PlaidLinkPublicTokenReceivedHandler.LinkFinishedPipeName.
    private const string LinkFinishedPipeName = "BudgetingApp.PlaidLinkFinished";

    public MainViewModel()
    {
        NavigationFactory = new NavigationFactory(this);
    }

    public IList<MainViewModelBase> MainNavPages { get; set; } = [];

    public NavigationFactory NavigationFactory { get; }

    public string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool HasNoTransactions => Transactions.Any();


    public bool IsBusy { get; set; }

    public bool IsLinkingAccount { get; set; }

    public string StatusMessage { get; set; } = string.Empty;

    public ObservableCollection<Transaction> Transactions { get; set; } = [];

    [RelayCommand]
    private async Task LinkAccount(TopLevel? topLevel)
    {
        if (topLevel is null)
        {
            return;
        }

        IsLinkingAccount = true;

        try
        {
            var linkResponse = await userAccountService.CreateLinkAsync();

            if (!linkResponse.IsSuccess)
            {
                ErrorMessage = linkResponse.ErrorMessage;
                return;
            }

            var options = new WebAuthenticatorOptions(new Uri(linkResponse.Value!), new Uri(PublicBaseUrl));

            await WebAuthenticationBroker.AuthenticateAsync(topLevel, options);

            StatusMessage = "Finishing up your linked account...";
            await WaitForSyncedAccountsAsync();
        }
        finally
        {
            IsLinkingAccount = false;
            StatusMessage = string.Empty;
        }
    }

    // The webhook listener (BudgetingApp.Server) does the actual token exchange/sync once Plaid's
    // SESSION_FINISHED webhook arrives, then connects to this pipe to let us know it's done - so we
    // block on that single OS-level wait handle instead of guessing/polling.
    private async Task WaitForSyncedAccountsAsync()
    {
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        await using var pipeServer = new NamedPipeServerStream(LinkFinishedPipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

        try
        {
            await pipeServer.WaitForConnectionAsync(timeoutCts.Token);

            /*
            Accounts = [.. await userAccountService.GetAccountsAsync()];
            */
            Transactions = [.. await userAccountService.GetTransactionsAsync()];
        }
        catch (OperationCanceledException)
        {
            // No signal within the timeout window - leave whatever was last synced (if anything) as-is.
            // The user can still hit the manual "Sync" command.
        }
    }

    [RelayCommand]
    private async Task Loaded()
    {
        /*_ = await userAccountService.TryToBindToUserAsync().ConfigureAwait(true);

        var accounts = await userAccountService.GetAccountsAsync();
        Accounts = [with(accounts)];

        var transactions = await userAccountService.GetTransactionsAsync();
        Transactions = [with(transactions)];*/
    }

    [RelayCommand]
    private async Task Sync()
    {
        /*var accounts = await userAccountService.GetAccountsAsync();
        Accounts = [with(accounts)];*/

        var transactions = await userAccountService.GetTransactionsAsync();
        Transactions = [with(transactions)];
    }
}

public sealed class NavigationFactory : IFANavigationPageFactory
{
    public NavigationFactory(MainViewModel owner)
    {
        Owner = owner;
    }

    public MainViewModel Owner { get; }

    public Control GetPage(Type srcType)
    {
        return null;
    }

    public Control GetPageFromObject(object target)
    {
        if (target is AccountOverviewPageViewModel)
        {
            return new AccountOverviewPage()
            {
                DataContext = target
            };
        }
        else if (target is TransactionsPageViewModel)
        {
            return new TransactionsPage()
            {
                DataContext = target
            };
        }
        else
        {
            return default;
        }
    }
}

