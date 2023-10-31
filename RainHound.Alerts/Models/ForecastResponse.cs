using System.Text.Json.Serialization;
using RainHound.Alerts.Models.Shared;

namespace RainHound.Alerts.Models;

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