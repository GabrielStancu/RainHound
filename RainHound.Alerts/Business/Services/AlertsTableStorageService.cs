using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
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
}
