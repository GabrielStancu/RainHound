using RainHound.Alerts.Models;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IForecastService
{
    Task<ForecastResponse?> GetForecastAsync(ForecastRequestModel forecastRequest);
}