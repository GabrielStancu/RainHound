using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services;

public class AlertsChecker : IAlertsChecker
{
    public async Task<List<FoundAlertModel>> CheckAlertsAsync(ForecastResponse forecast, IEnumerable<AlertModel> alertThresholds)
    {
        await Task.Delay(10);
        return new List<FoundAlertModel>();
    }
}
