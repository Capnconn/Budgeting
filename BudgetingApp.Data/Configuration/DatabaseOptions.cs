using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Infrastructure.Configuration
{
    public class DatabaseOptions
    {
        private static readonly string DatabaseName = "BudgetingApp.db";

        public static string OptionsName => "DatabaseOptions";

        public string? DatabasePath { get; set; } = null;
        public string DatabaseFullName => Path.Join(DatabasePath, DatabaseName);
    }
}
