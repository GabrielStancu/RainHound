namespace RainHound.Alerts.Models.FoundAlerts;

public class MinTempFoundAlertModel : FoundAlertModel
{
    public override string Description(double minTemp, double threshold) 
        => $"Minimum temperature ({minTemp}) below threshold ({threshold})";
}
