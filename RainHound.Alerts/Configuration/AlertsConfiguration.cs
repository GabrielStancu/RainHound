namespace RainHound.Alerts.Configuration;

public class AlertsConfiguration
{
    public string? ScheduleCron { get; set; }
    public string? FromEmail { get; set; }
    public string? Subject { get; set; }
    public string? ConnectionString { get; set; }
}
