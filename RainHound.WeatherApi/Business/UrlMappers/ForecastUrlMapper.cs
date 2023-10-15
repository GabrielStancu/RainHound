using RainHound.WeatherApi.Business.UrlBuilders;
using RainHound.WeatherApi.Contracts.Requests;

namespace RainHound.WeatherApi.Business.UrlMappers;

public static class ForecastUrlMapper
{
    public static string Build(ForecastRequest request)
    {
        string isAirQualityRequired = BoolToYesNoConverter.Convert(request.IsAirQualityRequired);
        string areAlertsRequired = BoolToYesNoConverter.Convert(request.AreAlertsRequired);

        return $"forecast.json?key={request.Key}&q={request.City}&days={request.Days}&aqi={isAirQualityRequired}&alerts={areAlertsRequired}";
    }
}