using System.Text.Json.Serialization;
using RainHound.WeatherApi.Contracts.Responses.Shared;

namespace RainHound.WeatherApi.Contracts.Responses;

public class WeatherResponse
{
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("current")]
    public CurrentState? CurrentState { get; set;}
}
