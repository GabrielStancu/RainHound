namespace RainHound.Alerts.Models.FoundAlerts;

public class ChancesOfRainFoundAlertModel : FoundAlertModel
{
    public override string Description(double chanceOfRain, double threshold) 
        => $"Chances of rain ({chanceOfRain}) above threshold ({threshold})";
}
