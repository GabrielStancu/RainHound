namespace RainHound.WeatherApi.Configuration;

public class AlertsFunctionConfiguration : BaseConfiguration
{
    public new static string SectionName => "AlertsFunction";
    public string? BaseUrl { get; set; }
}
