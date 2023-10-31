using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services;
public class AlertsTableStorageService : IAlertsTableStorageService
{
    private readonly TableClient _tableClient;
    private readonly ILogger _logger;

    public AlertsTableStorageService(TableClient tableClient, ILoggerFactory loggerFactory)
    {
        _tableClient = tableClient;
        _logger = loggerFactory.CreateLogger<AlertsTableStorageService>();
    }

    public async Task<Response> UpsertAlertAsync(AlertEntity alert)
    {
        _logger.LogInformation($"Adding alert entity with PartitionKey {alert.PartitionKey}, RowKey {alert.RowKey}");
        var response = await _tableClient.UpsertEntityAsync(alert);

        return response;
    }

    public async Task<Dictionary<string, List<AlertEntity>>> GetAlertsGroupedByCityAsync()
    {
        _logger.LogInformation("Looking for alerts...");
        var tableAlerts = _tableClient.QueryAsync<AlertEntity>();
        var cityAlerts = new Dictionary<string, List<AlertEntity>>();

        await foreach (var alertsPage in tableAlerts.AsPages())
        {
            foreach (var alert in alertsPage.Values)
            {
                GroupAlertsPageByCity(alert, cityAlerts);
            }
        }

        return cityAlerts;
    }

    private void GroupAlertsPageByCity(AlertEntity alert, IDictionary<string, List<AlertEntity>> cityAlerts)
    {
        if (string.IsNullOrEmpty(alert.City))
        {
            _logger.LogWarning($"Found empty city for row with key {alert.RowKey}");
            return;
        }

        _logger.LogWarning($"Found alert with row key {alert.RowKey} for city {alert.City}");

        if (cityAlerts.ContainsKey(alert.City))
        {
            cityAlerts[alert.City].Add(alert);
        }
        else
        {
            cityAlerts.Add(alert.City, new List<AlertEntity> { alert });
        }
    }
}
