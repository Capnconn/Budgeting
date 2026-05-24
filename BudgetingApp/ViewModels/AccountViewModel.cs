using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using BudgetingApp.Enums;
using BudgetingApp.Infrastructure.Data;
using BudgetingApp.Interfaces;
using BudgetingApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BudgetingApp.ViewModels;

public partial class LinkedAccountViewModel : ObservableObject
{
    [ObservableProperty] private string _accountName = string.Empty;
    [ObservableProperty] private string _bankName = string.Empty;
}

public partial class AccountViewModel : ObservableObject, IPageViewModel
{
    private readonly IUserAccountService _userAccountService;

    [ObservableProperty]
    public partial string FirstName { get; set; } = "Connor";

    [ObservableProperty]
    public partial string LastName { get; set; } = "Richey";

    [ObservableProperty]
    public partial string Email { get; set; } = "user@example.com";

    [ObservableProperty]
    public partial string Username { get; set; } = "Capnconn";

    public AccountViewModel(IUserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool ShowSuccess { get; set; }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    [ObservableProperty]
    public partial bool HasNoLinkedAccounts { get; set; } = false;

    public ObservableCollection<LinkedAccountViewModel> LinkedAccounts { get; } = new();

    public AppPage Page => AppPage.Account;
    public string DisplayName => Page.ToString();

    [RelayCommand]
    private void Discard()
    {
    }

    [RelayCommand]
    private async Task Save()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            ErrorMessage += "First and last name are required.";
        }

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage += "\n\n";

            ErrorMessage += "Please enter a valid email address.";
        }

        if (!string.IsNullOrWhiteSpace(ErrorMessage))
        {
            return;
        }

        var result = await _userAccountService.CreateUserAsync(FirstName, LastName, Email);

        if (!result.IsSuccess)
        {
            ErrorMessage = result.ErrorMessage;
            OnPropertyChanged(nameof(HasError));
        }

        DisplaySuccessPopup();
    }

    [RelayCommand]
    private async Task LinkAccount()
    {
        var linkResponse = await _userAccountService.CreateLinkAsync();

        if (!linkResponse.IsSuccess)
        {
            ErrorMessage = linkResponse.ErrorMessage;
            return;
        }

        var mainWindow = (App.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

        var plaidLinkPopup = new PlaidLinkPopup(linkResponse.Value!);
        plaidLinkPopup.HandleCloseEvent += HandleCloseEvent;
        await plaidLinkPopup.ShowDialog(mainWindow!);
    }

    private void DisplaySuccessPopup()
    {
        ShowSuccess = true;

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        timer.Tick += (_, _) =>
        {
            ShowSuccess = false;
            timer.Stop();
        };
        timer.Start();
    }

    private async Task HandleCloseEvent()
    {
        await _userAccountService.ProcessPlaidLinkAsync();
    }
}