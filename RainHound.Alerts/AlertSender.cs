using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts
{
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
        public async Task Run([TimerTrigger("10 38 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var alerts = await _alertsTableStorageService.GetAlertsGroupedByCityAsync();
            IEnumerable<FoundAlertModel> emailAlerts = new List<FoundAlertModel>();

            _logger.LogInformation("Fetched alert thresholds grouped by city. Processing alerts...");

            foreach (var alert in alerts)
            {
                var alertModels = alert.Value.Select(AlertEntityMapper.MapToAlert);
                var foundAlerts = await _alertsProcessor.GetAlertsForCityAsync(alert.Key, alertModels);
                emailAlerts = emailAlerts.Concat(foundAlerts);
            }

            await _alertsProcessor.SendAlertsAsync(emailAlerts);

            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus?.Next}");
        }
    }
}
