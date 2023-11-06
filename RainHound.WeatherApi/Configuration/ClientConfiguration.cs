namespace RainHound.WeatherApi.Configuration;

public class ClientConfiguration : BaseConfiguration
{
    public new static string SectionName => "Client";
    public string? BaseUrl { get; set; }
}
