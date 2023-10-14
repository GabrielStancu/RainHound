using RainHound.WeatherApi.Business.UrlBuilders;
using RainHound.WeatherApi.Contracts.Requests;

namespace RainHound.WeatherApi.Business.UrlMappers;

public interface IForecastUrlMapper
{
    string Build(ForecastRequest request);
}

public class ForecastUrlMapper : IForecastUrlMapper
{
    public string Build(ForecastRequest request)
    {
        string isAirQualityRequired = BoolToYesNoConverter.Convert(request.IsAirQualityRequired);
        string areAlertsRequired = BoolToYesNoConverter.Convert(request.AreAlertsRequired);

        return $"v1/forecast.json?key={request.Key}" +
            $"&q={request.City}&days={request.Days}&aqi={isAirQualityRequired}&alerts={areAlertsRequired}";
    }
}