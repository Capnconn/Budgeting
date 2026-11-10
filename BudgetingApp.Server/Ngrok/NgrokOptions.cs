namespace BudgetingApp.Server.Ngrok;

public class NgrokOptions
{
    public static string SectionName => "Ngrok";

    // Off by default (see appsettings.json) - a real deployment has a real public hostname and
    // should never try to shell out to ngrok. appsettings.Development.json turns this on locally.
    public bool Enabled { get; set; }
    public string ExecutablePath { get; set; } = "ngrok";
    public string Domain { get; set; } = string.Empty;
    public int Port { get; set; }
}
