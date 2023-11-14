using System.Collections.Concurrent;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Entities;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts;

public class AlertSender
{
    private readonly IAlertsTableStorageService _alertsTableStorageService;
    private readonly IAlertsProcessor _alertsProcessor;
    private readonly ILogger _logger;

    public AlertSender(IAlertsTableStorageService alertsTableStorageService, 
        IAlertsProcessor alertsProcessor,
        ILoggerFactory loggerFactory)
    {
        _alertsTableStorageService = alertsTableStorageService;
        _alertsProcessor = alertsProcessor;
        _logger = loggerFactory.CreateLogger<AlertSender>();
    }

    [Function("AlertSender")]
    public async Task Run([TimerTrigger("%Alerts:ScheduleCron%")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        var alertThresholdsForCities = await _alertsTableStorageService.GetAlertsGroupedByCityAsync();
        var alertsToEmail = new ConcurrentBag<FoundAlertModel>();
        var alertTasks = new List<Task>();

        _logger.LogInformation("Fetched alert thresholds grouped by city. Processing alerts...");

        foreach (var alertThresholdForCity in alertThresholdsForCities)
        {
            var alertTask = FindAlertsAsync(alertThresholdForCity, alertsToEmail);
            alertTasks.Add(alertTask);
        }

        await Task.WhenAll(alertTasks);
        await _alertsProcessor.SendAlertsAsync(alertsToEmail);

        _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus?.Next}");
    }

    private async Task FindAlertsAsync(KeyValuePair<string, List<AlertEntity>> alertThresholdsForCity, ConcurrentBag<FoundAlertModel> emailAlerts)
    {
        var alertsForCity = alertThresholdsForCity.Value.Select(AlertEntityMapper.MapToAlert);
        var foundAlerts = await _alertsProcessor.GetAlertsForCityAsync(alertThresholdsForCity.Key, alertsForCity);

        foreach (var foundAlert in foundAlerts)
        {
            emailAlerts.Add(foundAlert);
        }
    }
}