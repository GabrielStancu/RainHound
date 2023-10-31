using System.Text.Json.Serialization;

namespace RainHound.Alerts.Models.Shared;

public class ForecastDay
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("date_epoch")]
    public long DateEpoch { get; set; }

    [JsonPropertyName("day")]
    public Day? Day { get; set; }

    [JsonPropertyName("astro")]
    public Astro? Astro { get; set; }

    [JsonPropertyName("hour")]
    public List<Hour>? Hours { get; set; }
}
