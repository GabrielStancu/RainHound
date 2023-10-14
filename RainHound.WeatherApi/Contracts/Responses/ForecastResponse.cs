using System.Text.Json.Serialization;
using RainHound.WeatherApi.Contracts.Responses.Shared;

namespace RainHound.WeatherApi.Contracts.Responses;

public class ForecastResponse
{
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("current")]
    public CurrentState? CurrentState { get; set; }

    [JsonPropertyName("forecast")]
    public Forecast? Forecast { get; set; }

    [JsonPropertyName("alerts")]
    public dynamic? Alerts { get; set; } // TODO: see what this looks like when received
}