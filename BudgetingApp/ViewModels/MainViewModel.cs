using BudgetingApp.Enums;
using BudgetingApp.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace BudgetingApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const AppPage StartingPage = AppPage.Account;

    public MainViewModel(AccountViewModel accountViewModel, TransactionsViewModel transactionViewModel)
    {
        Pages =
        [
            accountViewModel,
            transactionViewModel,
            // Add more page ViewModels here as your app grows, e.g.:
            // dashboardViewModel,
            // transactionsViewModel,
        ];

        CurrentPage = Pages.First(x => x.Page == StartingPage);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentPageTitle))]
    public partial IPageViewModel? CurrentPage { get; set; }

    public ObservableCollection<IPageViewModel> Pages { get; }

    // Exposed so the nav header could bind to it if needed
    public string CurrentPageTitle => CurrentPage?.DisplayName ?? string.Empty;

    [RelayCommand]
    private void Loaded()
    {
        CurrentPage = Pages.First(x => x.Page == StartingPage);
    }
}