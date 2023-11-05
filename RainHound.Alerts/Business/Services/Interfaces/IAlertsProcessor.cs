using RainHound.Alerts.Models;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IAlertsProcessor
{
    Task<List<FoundAlertModel>> GetAlertsForCityAsync(string city, IEnumerable<AlertModel> alertsForCity);
    Task SendAlertsAsync(IEnumerable<FoundAlertModel> emailAlerts);
}