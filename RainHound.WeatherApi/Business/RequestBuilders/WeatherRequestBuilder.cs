using RainHound.WeatherApi.Contracts.Requests;

namespace RainHound.WeatherApi.Business.RequestBuilders;

public class WeatherRequestBuilder
{
    private WeatherRequest _weatherRequest;

    public WeatherRequestBuilder(string key, string city)
    {
        _weatherRequest = new WeatherRequest
        {
            Key = key,
            City = city
        };
    }

    public WeatherRequestBuilder IsAirQualityRequired(bool isAirQualityRequired)
    {
        _weatherRequest.IsAirQualityRequired = isAirQualityRequired;
        return this;
    }

    public WeatherRequest Build()
    {
        return _weatherRequest;
    }
}
