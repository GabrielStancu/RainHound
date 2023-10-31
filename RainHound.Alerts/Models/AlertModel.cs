namespace RainHound.Alerts.Models;

public class AlertModel
{
    public string? Email { get; set; }
    public int? MinTemp { get; set; }
    public int? MaxTemp { get; set; }
    public int? ChancesOfRain { get; set; }
    public string? City { get; set; }
}
