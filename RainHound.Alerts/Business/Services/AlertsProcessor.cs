using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models.FoundAlerts;
using RainHound.Alerts.Models;
using Microsoft.Extensions.Logging;

namespace RainHound.Alerts.Business.Services;
public class AlertsProcessor : IAlertsProcessor
{
    private readonly IForecastService _forecastService;
    private readonly IAlertsChecker _alertsChecker;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AlertsProcessor> _logger;

    public AlertsProcessor(IForecastService forecastService,
        IAlertsChecker alertsChecker,
        IEmailSender emailSender,
        ILogger<AlertsProcessor> logger)
    {
        _forecastService = forecastService;
        _alertsChecker = alertsChecker;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<List<FoundAlertModel>> GetAlertsForCityAsync(string city, IEnumerable<AlertModel> alertsForCity)
    {
        _logger.LogInformation($"Fetching forecast data for city {city}");

        var forecastModel = new ForecastRequestModel { City = city };
        var forecastResponse = await _forecastService.GetForecastAsync(forecastModel);

        if (forecastResponse is null)
        {
            _logger.LogError($"Error while fetching forecast for {city}");
            return Enumerable.Empty<FoundAlertModel>().ToList();
        }

        _logger.LogInformation("Fetched forecast data. Checking for alerts thresholds...");
        var foundAlerts = _alertsChecker.CheckAlerts(forecastResponse, alertsForCity);
        _logger.LogInformation($"Found alerts for city {city}: {foundAlerts.Count}");

        return foundAlerts;
    }

    public async Task SendAlertsAsync(IEnumerable<FoundAlertModel> emailAlerts)
    {
        var alertsGroupedByEmail = emailAlerts.GroupBy(a => a.Email);

        foreach (var alertsGroup in alertsGroupedByEmail)
        {
            if (string.IsNullOrEmpty(alertsGroup.Key))
                continue;

            _logger.LogInformation($"Preparing email alerts to be sent to {alertsGroup.Key}");
            await _emailSender.SendEmailToUserAsync(alertsGroup.ToList());
        }
    }
}
