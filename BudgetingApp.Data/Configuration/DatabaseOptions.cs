using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Data.Configuration;

public class DatabaseOptions
{
    private static readonly string DatabaseName = "BudgetingApp.db";

    public static string OptionsName => "DatabaseOptions";

    public string? DatabasePath { get; init; } = null;
    public string DatabaseFullName => Path.Join(DatabasePath, DatabaseName);
}
