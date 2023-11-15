using System.Text.Json.Serialization;

namespace RainHound.Alerts.Models;

public class AlertModel
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("minTemp")]
    public double? MinTemp { get; set; }

    [JsonPropertyName("maxTemp")]
    public double? MaxTemp { get; set; }

    [JsonPropertyName("chancesOfRain")]
    public double? ChancesOfRain { get; set; }
}
