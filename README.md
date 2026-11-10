# Budgeting

A simple, free budgeting app for our household. Bank data comes from [Plaid](https://plaid.com/); the focus is on budgeting, spending tracking, and building better habits.

Work is tracked in [GitHub Issues](https://github.com/Capnconn/Budgeting/issues), grouped into milestones.

## Projects

| Project | Purpose |
| --- | --- |
| `BudgetingApp` | Shared Avalonia UI (views, view models) |
| `BudgetingApp.Desktop` | Windows/macOS/Linux desktop head |
| `BudgetingApp.Android`, `BudgetingApp.iOS`, `BudgetingApp.Browser` | Other platform heads (parked for now) |
| `BudgetingApp.Server` | ASP.NET Core host: Plaid webhooks, token exchange, sync |
| `BudgetingApp.Services` | Application logic |
| `BudgetingApp.Adapter` | Plaid API wrapper (Going.Plaid) |
| `BudgetingApp.Data` | EF Core + SQLite entities, migrations, repositories |
| `BudgetingApp.Common` | Shared helpers (`Result`, extensions) |

## Local setup

Requires the .NET 10 SDK.

Plaid keys and machine-specific paths are **not** committed. Create an `appsettings.Local.json` (git-ignored) next to `appsettings.json` in both `BudgetingApp.Desktop` and `BudgetingApp.Server`:

```json
{
  "DatabaseOptions": {
    "DatabasePath": "C:\\path\\to\\a\\folder"
  },
  "Plaid": {
    "ClientId": "<your Plaid client id>",
    "Secret": "<your Plaid secret>"
  }
}
```

Both projects must point at the same `DatabasePath` — they share one SQLite database.

Run:

```
dotnet run --project BudgetingApp.Server
dotnet run --project BudgetingApp.Desktop
```

The server needs a public URL for Plaid webhooks. In Development it can start an ngrok tunnel for you; see the `Ngrok` section of `BudgetingApp.Server/appsettings.json`.
