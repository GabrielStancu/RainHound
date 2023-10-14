namespace RainHound.WeatherApi.Contracts.Requests;

public class WeatherRequest
{
    public string Key { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public bool IsAirQualityRequired { get; set; }
}
