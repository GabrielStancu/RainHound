using System.Text.Json;
using RainHound.WeatherApi.Business.RequestBuilders;
using RainHound.WeatherApi.Business.UrlMappers;
using RainHound.WeatherApi.Configuration;
using RainHound.WeatherApi.Contracts.Requests;
using RainHound.WeatherApi.Contracts.Responses;
using RestSharp;

namespace RainHound.WeatherApi.Services;

public class WeatherService
{
    private readonly WeatherApiConfiguration _weatherApiConfiguration;
    private readonly RestClient _client;
    private readonly IForecastUrlMapper _forecastUrlBuilder;

    public WeatherService(WeatherApiConfiguration weatherApiConfiguration,
        IForecastUrlMapper forecastUrlMapper)
    {
        _weatherApiConfiguration = weatherApiConfiguration;
        _client ??= new RestClient(weatherApiConfiguration.BaseUrl);
        _forecastUrlBuilder = forecastUrlMapper;
    }

    public async Task<ForecastResponse?> GetForecastAsync(string city, int days, bool isAirQualityRequired, bool areAlertsRequired)
    {
        var forecastRequest = new ForecastRequestBuilder(_weatherApiConfiguration.ApiKey, city)
            .IsAirQualityRequired(isAirQualityRequired)
            .AreAlertsRequired(areAlertsRequired)
            .ForDays(days)
            .Build();
        var requestUrl = _forecastUrlBuilder.Build(forecastRequest);
        var request = new RestRequest(requestUrl);
        var response = await _client.ExecuteGetAsync(request);

        if (!response.IsSuccessful)
        {
            // Log here
            return null;
        }

        var forecastResponse = JsonSerializer.Deserialize<ForecastResponse>(response.Content ?? string.Empty);
        return forecastResponse;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string city, bool isAirQualityRequired)
    {
        // TODO
        return null;
    }
}
