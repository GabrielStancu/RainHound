namespace RainHound.Alerts.Requests;

public class SetAlertRequest
{
    public string? Email { get; set; }
    public int? MinTemp { get; set; }
    public int? MaxTemp { get; set; }
    public int? ChancesOfRain { get; set; }
}
