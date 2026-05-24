using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Threading.Tasks;
using System.Web;

namespace BudgetingApp;

public partial class PlaidLinkPopup : Window
{
    private readonly string _hostedLinkUrl;

    public PlaidLinkPopup(string hostedLinkUrl)
    {
        InitializeComponent();
        _hostedLinkUrl = hostedLinkUrl;
    }

    public string? AccessToken { get; private set; }

    public Func<Task>? HandleCloseEvent = null;

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        PlaidWebView.NavigationStarted += async (s, args) =>
        {
            if (args.Request?.Equals("https://budget.local/") is true)
            {
                args.Cancel = true;

                var query = HttpUtility.ParseQueryString(args.Request.Query);
                AccessToken = query["public_token"];

                Close();
            }
        };

        PlaidWebView.Source = new Uri(_hostedLinkUrl);
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
        HandleCloseEvent?.Invoke();
    }
}