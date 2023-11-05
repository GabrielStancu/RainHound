using System.Text.Json;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models;
using RainHound.Alerts.Models.FoundAlerts;
using RainHound.Alerts.Models.Shared;

namespace RainHound.Alerts.Business.Services;

public class AlertsChecker : IAlertsChecker
{
    private readonly ILogger<AlertsChecker> _logger;

    public AlertsChecker(ILogger<AlertsChecker> logger)
    {
        _logger = logger;
    }

    public List<FoundAlertModel> CheckAlerts(ForecastResponse forecast, IEnumerable<AlertModel> alertThresholds)
    {
        var alerts = new List<FoundAlertModel>();
        alertThresholds = alertThresholds.ToList();

        if (forecast.Forecast?.ForecastDays is null)
        {
            _logger.LogError($"Invalid forecast: <{JsonSerializer.Serialize(forecast)}>");
            return Enumerable.Empty<FoundAlertModel>().ToList();
        }

        foreach (var day in forecast.Forecast.ForecastDays)
        {
            if (day.Hours is null)
            {
                _logger.LogError($"Invalid hours for forecast: <{JsonSerializer.Serialize(forecast)}>");
                continue;
            }

            foreach (var hour in day.Hours)
            {
                foreach (var alertThreshold in alertThresholds)
                {
                    CheckAlertThresholdForHour(alertThreshold, hour, alerts);
                }
            }
        }

        return alerts;
    }

    private void CheckAlertThresholdForHour(AlertModel alertThreshold, Hour hour, List<FoundAlertModel> alerts)
    {
        if (alertThreshold.ChancesOfRain != null && hour.ChanceOfRain > alertThreshold.ChancesOfRain)
        {
            AddChanceOfRainAlert(alertThreshold, hour, alerts);
        }

        if (alertThreshold.MinTemp != null && hour.TempC < alertThreshold.MinTemp)
        {
            AddMinTempAlert(alertThreshold, hour, alerts);
        }

        if (alertThreshold.MaxTemp != null && hour.TempC > alertThreshold.MaxTemp)
        {
            AddMaxTempAlert(alertThreshold, hour, alerts);
        }
    }

    private void AddChanceOfRainAlert(AlertModel alertThreshold, Hour hour, List<FoundAlertModel> alerts)
    {
        var alert = new ChancesOfRainFoundAlertModel
        {
            City = alertThreshold.City,
            Email = alertThreshold.Email,
            Hour = DateTime.Parse(hour.Time),
            Threshold = alertThreshold.ChancesOfRain ?? double.NegativeZero,
            Value = hour.ChanceOfRain
        };

        _logger.LogInformation($"Found chance of rain alert: {JsonSerializer.Serialize(alert)}");
        alerts.Add(alert);
    }

    private void AddMinTempAlert(AlertModel alertThreshold, Hour hour, List<FoundAlertModel> alerts)
    {
        var alert = new MinTempFoundAlertModel
        {
            City = alertThreshold.City,
            Email = alertThreshold.Email,
            Hour = DateTime.Parse(hour.Time),
            Threshold = alertThreshold.MinTemp ?? double.NegativeZero,
            Value = hour.TempC
        };

        _logger.LogInformation($"Found min temp alert: {JsonSerializer.Serialize(alert)}");
        alerts.Add(alert);
    }

    private void AddMaxTempAlert(AlertModel alertThreshold, Hour hour, List<FoundAlertModel> alerts)
    {
        var alert = new MinTempFoundAlertModel
        {
            City = alertThreshold.City,
            Email = alertThreshold.Email,
            Hour = DateTime.Parse(hour.Time),
            Threshold = alertThreshold.MaxTemp ?? double.NegativeZero,
            Value = hour.TempC
        };

        _logger.LogInformation($"Found max temp alert: {JsonSerializer.Serialize(alert)}");
        alerts.Add(alert);
    }
}
