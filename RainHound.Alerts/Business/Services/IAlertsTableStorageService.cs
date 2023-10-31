using Azure;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services;

public interface IAlertsTableStorageService
{
    Task<Response> UpsertAlertAsync(AlertEntity alert);
}