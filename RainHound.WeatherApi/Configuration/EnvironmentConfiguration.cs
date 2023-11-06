namespace RainHound.WeatherApi.Configuration;

public class EnvironmentConfiguration : BaseConfiguration
{
    public new static string SectionName => "Environment";
    public string Name { get; set; } = string.Empty;
}
