namespace RainHound.Alerts.Configuration;

public class AlertsStorageConfiguration
{
    public const string SectionName = "AlertsStorage";
    public string? ConnectionString { get; set; }
    public string? TableStorage { get; set; }
}
