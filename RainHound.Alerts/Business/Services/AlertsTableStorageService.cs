using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Configuration;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services;

public class AlertsTableStorageService : IAlertsTableStorageService
{
    private readonly TableStorageConfiguration _configuration;
    private TableClient? _tableClient;
    private readonly ILogger _logger;
    private const string TableName = "Alerts";

    public AlertsTableStorageService(TableStorageConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _logger = loggerFactory.CreateLogger<AlertsTableStorageService>();
    }

    public async Task<Response> UpsertAlertAsync(AlertEntity alert)
    {
        await InitTableClientAsync();

        _logger.LogInformation("Upserting alert entity with PartitionKey {PartitionKey}, RowKey {RowKey}", alert.PartitionKey, alert.RowKey);
        var response = await _tableClient!.UpsertEntityAsync(alert);
        _logger.LogInformation("Upserted alert entity");

        return response;
    }

    public async Task<Dictionary<string, List<AlertEntity>>> GetAlertsGroupedByCityAsync()
    {
        await InitTableClientAsync();

        _logger.LogInformation("Looking for alerts...");
        var tableAlerts = _tableClient!.QueryAsync<AlertEntity>();
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

    private async Task InitTableClientAsync()
    {
        _logger.LogInformation("Initializing table client for {TableName}", TableName);

        if (_tableClient != null)
        {
            _logger.LogInformation("Table already initialized, skipping...");
            return;
        }

        var serviceClient = new TableServiceClient(_configuration.ConnectionString);
        _tableClient = serviceClient.GetTableClient(TableName);
        await _tableClient.CreateIfNotExistsAsync();

        _logger.LogInformation("Created table client for {TableName}", TableName);
    }

    private void GroupAlertsPageByCity(AlertEntity alert, IDictionary<string, List<AlertEntity>> cityAlerts)
    {
        if (string.IsNullOrEmpty(alert.PartitionKey))
        {
            _logger.LogWarning("Found empty city for row with key {RowKey}", alert.RowKey);
            return;
        }

        _logger.LogInformation("Found alert with email {RowKey} for city {PartitionKey}", alert.RowKey, alert.PartitionKey);

        if (cityAlerts.ContainsKey(alert.PartitionKey))
        {
            cityAlerts[alert.PartitionKey].Add(alert);
        }
        else
        {
            cityAlerts.Add(alert.PartitionKey, new List<AlertEntity> { alert });
        }
    }
}
