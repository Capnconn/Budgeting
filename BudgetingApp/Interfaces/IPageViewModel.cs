using BudgetingApp.Enums;

namespace BudgetingApp.Interfaces
{
    public interface IPageViewModel
    {
        AppPage Page { get; }
        string DisplayName { get; }
    }
}
