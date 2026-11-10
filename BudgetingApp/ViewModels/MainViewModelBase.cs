namespace BudgetingApp.ViewModels;

public abstract class MainViewModelBase : ViewModelBase
{
    public MainViewModel Parent { get; set; }

    public abstract string IconKey { get; }

    public abstract string NavHeader { get; }

    public abstract bool ShowsInFooter { get; }
}
