namespace RainHound.Alerts.Models;
public class ForecastRequestModel
{
    public string? City { get; set; }
    public int Days { get; } = 1;
    public bool Aqi { get; } = false;
}
