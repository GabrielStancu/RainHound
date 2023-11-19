namespace RainHound.WeatherApi.Configuration;

public class WeatherApiConfiguration : BaseConfiguration
{
    public new static string SectionName => "WeatherApi";
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int CacheDurationMinutes { get; set; }
}