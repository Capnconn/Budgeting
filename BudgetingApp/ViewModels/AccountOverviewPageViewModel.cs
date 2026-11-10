using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Going.Plaid.Entity;

namespace BudgetingApp.ViewModels;

public partial class AccountOverviewPageViewModel : MainViewModelBase
{
    [ObservableProperty]
    public partial ObservableCollection<Account> Accounts { get; set; }

    public override string IconKey => "AccountOverviewPageIcon";
    public override string NavHeader => "Account Overview";
    public override bool ShowsInFooter => false;
}
