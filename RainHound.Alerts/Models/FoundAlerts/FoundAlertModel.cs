namespace RainHound.Alerts.Models.FoundAlerts;
public abstract class FoundAlertModel
{
    public string? Email { get; set; }
    public string? City { get; set; }
    public DateTime Hour { get; set; }
    public decimal Threshold { get; set; }
    public decimal Value { get; set; }
    public abstract string? Description { get; }
}
