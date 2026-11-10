namespace BudgetingApp.Services.Data;

public record CachedUser(
    Guid Id,
    string Username,
    string Name,
    string Email
);