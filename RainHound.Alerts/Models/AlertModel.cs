namespace RainHound.Alerts.Models;

public class AlertModel
{
    public string? Email { get; set; }
    public string? City { get; set; }
    public double? MinTemp { get; set; }
    public double? MaxTemp { get; set; }
    public double? ChancesOfRain { get; set; }
}
