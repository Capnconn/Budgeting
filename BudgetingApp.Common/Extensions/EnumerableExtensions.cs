using System.Collections.ObjectModel;

namespace BudgetingApp.Common.Extensions;

public static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list) where T : class
        => [.. list];
}
