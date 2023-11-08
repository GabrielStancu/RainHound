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

        var groupedAlerts = GroupConsecutiveAlerts(alerts);

        _logger.LogInformation($"Alerts after grouping: {JsonSerializer.Serialize(groupedAlerts)}");

        return groupedAlerts;
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
            StartHour = DateTime.Parse(hour.Time),
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
            StartHour = DateTime.Parse(hour.Time),
            Threshold = alertThreshold.MinTemp ?? double.NegativeZero,
            Value = hour.TempC
        };

        _logger.LogInformation($"Found min temp alert: {JsonSerializer.Serialize(alert)}");
        alerts.Add(alert);
    }

    private void AddMaxTempAlert(AlertModel alertThreshold, Hour hour, List<FoundAlertModel> alerts)
    {
        var alert = new MaxTempFoundAlertModel
        {
            City = alertThreshold.City,
            Email = alertThreshold.Email,
            StartHour = DateTime.Parse(hour.Time),
            Threshold = alertThreshold.MaxTemp ?? double.NegativeZero,
            Value = hour.TempC
        };

        _logger.LogInformation($"Found max temp alert: {JsonSerializer.Serialize(alert)}");
        alerts.Add(alert);
    }

    private static List<FoundAlertModel> GroupConsecutiveAlerts(List<FoundAlertModel> alerts)
    {
        var chanceOfRainAlerts = GroupAlertsByType<ChancesOfRainFoundAlertModel>(alerts);
        var minTempAlerts = GroupAlertsByType<MinTempFoundAlertModel>(alerts);
        var maxTempAlerts = GroupAlertsByType<MaxTempFoundAlertModel>(alerts);

        return chanceOfRainAlerts
            .Concat(minTempAlerts)
            .Concat(maxTempAlerts)
            .ToList();
    }

    private static IEnumerable<FoundAlertModel> GroupAlertsByType<T>(List<FoundAlertModel> alerts) where T : FoundAlertModel
    {
        var groupedAlerts = alerts
            .Where(a => a is T)
            .GroupBy(a => new
            {
                a.City,
                a.Email
            })
            .Select(g =>
                (g.ToList().FirstOrDefault(), g.ToList().LastOrDefault()))
            .Select(p =>
            {
                if (p.Item1 is null || p.Item2 is null)
                    return null;

                p.Item1.EndHour = p.Item2.StartHour.AddHours(1);
                return p.Item1;
            })
            .Where(a => a != null);

        return groupedAlerts!;
    }
}
