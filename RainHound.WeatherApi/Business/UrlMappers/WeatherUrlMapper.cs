using RainHound.WeatherApi.Contracts.Requests;

namespace RainHound.WeatherApi.Business.UrlBuilders;

public static class WeatherUrlMapper
{
    public static string Build(WeatherRequest request)
    {
        string isAirQualityRequired = BoolToYesNoConverter.Convert(request.IsAirQualityRequired);

        return $"current.json?key={request.Key}&q={request.City}&aqi={isAirQualityRequired}";
    }
}