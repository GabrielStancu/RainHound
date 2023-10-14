namespace RainHound.WeatherApi.Contracts.Requests;

public class ForecastRequest
{
    public string Key { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Days { get; set; }
    public bool IsAirQualityRequired { get; set; }
    public bool AreAlertsRequired { get; set; }
}
