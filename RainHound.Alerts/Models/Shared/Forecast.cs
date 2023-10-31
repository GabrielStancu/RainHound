using System.Text.Json.Serialization;

namespace RainHound.Alerts.Models.Shared;

public class Forecast
{
    [JsonPropertyName("forecastday")]
    public List<ForecastDay>? ForecastDays { get; set; }
}
