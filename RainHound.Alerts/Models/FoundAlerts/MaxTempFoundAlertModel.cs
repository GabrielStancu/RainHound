namespace RainHound.Alerts.Models.FoundAlerts;

public class MaxTempFoundAlertModel : FoundAlertModel
{
    public override string Description(double maxTemp, double threshold) 
        => $"Chances of rain ({maxTemp}) above threshold ({threshold})";
}
