using RainHound.WeatherApi.Contracts.Requests;

namespace RainHound.WeatherApi.Business.RequestBuilders;

public class ForecastRequestBuilder
{
    private ForecastRequest _forecastRequest;

    public ForecastRequestBuilder(string key, string city)
    {
        _forecastRequest = new ForecastRequest
        {
            Key = key,
            Days = 1,
            City = city
        };
    }

    public ForecastRequestBuilder IsAirQualityRequired(bool isAirQualityRequired)
    {
        _forecastRequest.IsAirQualityRequired = isAirQualityRequired;
        return this;
    }

    public ForecastRequestBuilder AreAlertsRequired(bool areAlertsRequired)
    {
        _forecastRequest.AreAlertsRequired = areAlertsRequired;
        return this;
    }

    public ForecastRequestBuilder ForDays(int days)
    {
        _forecastRequest.Days = days;
        return this;
    }

    public ForecastRequest Build()
    {
        return _forecastRequest;
    }
}