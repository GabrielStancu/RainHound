using RainHound.Alerts.Models;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IAlertsChecker
{
    Task<List<FoundAlertModel>> CheckAlertsAsync(ForecastResponse forecast, IEnumerable<AlertModel> alertThresholds);
}