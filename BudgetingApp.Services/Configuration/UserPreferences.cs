namespace BudgetingApp.Services.Configuration;

public class UserPreferences
{
    // TODO: Update this so that the user's username is captured.
    public Guid UserId { get; set; } 
    public string Email { get; set; } = string.Empty;
    public string Theme { get; set; } = "Light";
    // anything else you want persisted across sessions
}