namespace RainHound.WeatherApi.Configuration;

public class EnvironmentConfiguration : BaseConfiguration
{
    public static new string SectionName { get; } = "Environment";
    public string Name { get; set; } = string.Empty;
}
