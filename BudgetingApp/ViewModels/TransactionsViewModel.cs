using BudgetingApp.Enums;
using BudgetingApp.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BudgetingApp.ViewModels
{
    public class TransactionItem
    {
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string CategoryIcon { get; set; } = "";
        public string CategoryColor { get; set; } = "#F0F0F5";
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public string FormattedAmount =>
            Amount >= 0 ? $"+${Amount:F2}" : $"-${Math.Abs(Amount):F2}";

        public string AmountColor =>
            Amount >= 0 ? "#2ECC71" : "#E74C3C";
    }


    public class TransactionsViewModel : ObservableObject, IPageViewModel
    {
        public AppPage Page => AppPage.Transactions;

        public string DisplayName => Page.ToString();

        public ObservableCollection<TransactionItem> Transactions { get; } = new()
        {
            new TransactionItem { Description = "Whole Foods Market", Category = "Groceries",    CategoryIcon = "🛒", CategoryColor = "#E8F5E9", Date = DateTime.Today.AddDays(-1),  Amount = -64.30m  },
            new TransactionItem { Description = "Netflix",            Category = "Subscriptions", CategoryIcon = "🎬", CategoryColor = "#FCE4EC", Date = DateTime.Today.AddDays(-2),  Amount = -15.99m  },
            new TransactionItem { Description = "Salary Deposit",     Category = "Income",        CategoryIcon = "💰", CategoryColor = "#E3F2FD", Date = DateTime.Today.AddDays(-3),  Amount = 3200.00m },
            new TransactionItem { Description = "Shell Gas Station",  Category = "Transport",     CategoryIcon = "⛽", CategoryColor = "#FFF3E0", Date = DateTime.Today.AddDays(-4),  Amount = -48.75m  },
            new TransactionItem { Description = "Amazon",             Category = "Shopping",      CategoryIcon = "📦", CategoryColor = "#F3E5F5", Date = DateTime.Today.AddDays(-5),  Amount = -112.49m },
            new TransactionItem { Description = "Chipotle",           Category = "Dining",        CategoryIcon = "🌯", CategoryColor = "#FBE9E7", Date = DateTime.Today.AddDays(-6),  Amount = -13.85m  },
            new TransactionItem { Description = "Freelance Payment",  Category = "Income",        CategoryIcon = "💰", CategoryColor = "#E3F2FD", Date = DateTime.Today.AddDays(-7),  Amount = 500.00m  },
            new TransactionItem { Description = "Spotify",            Category = "Subscriptions", CategoryIcon = "🎵", CategoryColor = "#FCE4EC", Date = DateTime.Today.AddDays(-8),  Amount = -9.99m   },
        };

        public int TransactionCount => Transactions.Count;
    }
}
