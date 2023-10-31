using Azure;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IAlertsTableStorageService
{
    Task<Response> UpsertAlertAsync(AlertEntity alert);
    Task<Dictionary<string, List<AlertEntity>>> GetAlertsGroupedByCityAsync();
}