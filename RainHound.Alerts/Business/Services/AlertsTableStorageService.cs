﻿using Azure;
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

        _logger.LogInformation($"Adding alert entity with PartitionKey {alert.PartitionKey}, RowKey {alert.RowKey}");
        var response = await _tableClient!.UpsertEntityAsync(alert);

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
        if (_tableClient != null)
            return;

        var serviceClient = new TableServiceClient(_configuration.ConnectionString);

        _tableClient = serviceClient.GetTableClient(TableName);
        await _tableClient.CreateIfNotExistsAsync();
    }

    private void GroupAlertsPageByCity(AlertEntity alert, IDictionary<string, List<AlertEntity>> cityAlerts)
    {
        if (string.IsNullOrEmpty(alert.City))
        {
            _logger.LogWarning($"Found empty city for row with key {alert.RowKey}");
            return;
        }

        _logger.LogInformation($"Found alert with row key {alert.RowKey} for city {alert.City}");

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
