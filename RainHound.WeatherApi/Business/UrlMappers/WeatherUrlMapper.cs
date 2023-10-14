using RainHound.WeatherApi.Contracts.Requests;

namespace RainHound.WeatherApi.Business.UrlBuilders;

public interface IWeatherUrlMapper
{
    string Build(WeatherRequest request);
}

public class WeatherUrlMapper : IWeatherUrlMapper
{
    public string Build(WeatherRequest request)
    {
        string isAirQualityRequired = BoolToYesNoConverter.Convert(request.IsAirQualityRequired);

        return $"current.json?key={request.Key}&q={request.City}&aqi={isAirQualityRequired}";
    }
}