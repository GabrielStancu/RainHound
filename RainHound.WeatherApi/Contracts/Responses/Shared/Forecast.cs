using System.Text.Json.Serialization;

namespace RainHound.WeatherApi.Contracts.Responses.Shared;

public class Forecast
{
    [JsonPropertyName("forecastday")]
    public List<ForecastDay>? ForecastDays { get; set; }
}
