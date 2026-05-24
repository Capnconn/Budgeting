namespace BudgetingApp.Services.Data;

public record UserAccount(
    Guid Id,
    string Username,
    string Name,
    string Email
);