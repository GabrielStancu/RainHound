using RainHound.WeatherApi.Contracts.Responses;

namespace RainHound.WeatherApi.Services;

public interface IWeatherService
{
    Task<ForecastResponse?> GetForecastAsync(string city, int days, bool isAirQualityRequired, bool areAlertsRequired);
    Task<WeatherResponse?> GetWeatherAsync(string city, bool isAirQualityRequired);
}