using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using BudgetingApp.ViewModels;
using FluentAvalonia.UI.Controls;

namespace BudgetingApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var vm = new MainViewModel();
        DataContext = vm;
        FrameView.NavigationPageFactory = vm.NavigationFactory;
        InitializeNavigationPages();
    }

    private void InitializeNavigationPages()
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var mainPages = new MainViewModelBase[]
            {
                new AccountOverviewPageViewModel
                {
                    Parent = (DataContext as MainViewModel)!
                },
                new TransactionsPageViewModel
                {
                    Parent = (DataContext as MainViewModel)!
                }
            };

            var menuItems = new List<FANavigationViewItem>(mainPages.Count(x => !x.ShowsInFooter));
            var footerItems = new List<FANavigationViewItem>(mainPages.Count(x => x.ShowsInFooter));

            var dc = DataContext as MainViewModel;
            foreach (var mainPage in mainPages)
            {
                var page = new FANavigationViewItem
                {
                    Content = mainPage.NavHeader,
                    Tag = mainPage,
                    IconSource = this.FindResource(mainPage.IconKey) as FAIconSource
                };

                if (mainPage.ShowsInFooter)
                {
                    footerItems.Add(page);
                }
                else
                {
                    menuItems.Add(page);
                }
            }

            dc.MainNavPages = mainPages;
            NavView.MenuItemsSource = menuItems;
            NavView.FooterMenuItemsSource = footerItems;
        });
    }
}
