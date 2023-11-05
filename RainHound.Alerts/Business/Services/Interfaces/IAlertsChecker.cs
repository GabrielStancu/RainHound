using RainHound.Alerts.Models;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IAlertsChecker
{
    List<FoundAlertModel> CheckAlerts(ForecastResponse forecast, IEnumerable<AlertModel> alertThresholds);
}