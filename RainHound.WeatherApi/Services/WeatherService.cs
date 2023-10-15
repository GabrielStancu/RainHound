using System.Text.Json;
using RainHound.WeatherApi.Business.RequestBuilders;
using RainHound.WeatherApi.Business.UrlBuilders;
using RainHound.WeatherApi.Business.UrlMappers;
using RainHound.WeatherApi.Configuration;
using RainHound.WeatherApi.Contracts.Responses;
using RestSharp;

namespace RainHound.WeatherApi.Services;

public interface IWeatherService
{
    Task<ForecastResponse?> GetForecastAsync(string city, int days, bool isAirQualityRequired, bool areAlertsRequired);
    Task<WeatherResponse?> GetWeatherAsync(string city, bool isAirQualityRequired);
}

public class WeatherService : IWeatherService
{
    private readonly WeatherApiConfiguration _weatherApiConfiguration;
    private readonly ILogger<WeatherService> _logger;
    private readonly RestClient _client;

    public WeatherService(WeatherApiConfiguration weatherApiConfiguration, ILogger<WeatherService> logger)
    {
        _weatherApiConfiguration = weatherApiConfiguration;
        _logger = logger;
        _client ??= new RestClient(weatherApiConfiguration.BaseUrl);
    }

    public async Task<ForecastResponse?> GetForecastAsync(string city, int days, bool isAirQualityRequired, bool areAlertsRequired)
    {
        var forecastRequest = new ForecastRequestBuilder(_weatherApiConfiguration.ApiKey, city)
            .IsAirQualityRequired(isAirQualityRequired)
            .AreAlertsRequired(areAlertsRequired)
            .ForDays(days)
            .Build();

        _logger.LogTrace($"Getting forecast for request <{JsonSerializer.Serialize(forecastRequest)}>");

        var requestUrl = ForecastUrlMapper.Build(forecastRequest);

        _logger.LogTrace($"Calling the URL at {requestUrl}");

        var request = new RestRequest(requestUrl);
        var response = await _client.ExecuteGetAsync(request);

        if (!response.IsSuccessful || response.Content is null)
        {
            _logger.LogError($"Failed to get the weather. Response: {response.Content}");
            return null;
        }

        _logger.LogTrace($"Received response: {response.Content}");
        var forecastResponse = JsonSerializer.Deserialize<ForecastResponse>(response.Content);
        
        return forecastResponse;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string city, bool isAirQualityRequired)
    {
        var weatherRequest = new WeatherRequestBuilder(_weatherApiConfiguration.ApiKey, city)
            .IsAirQualityRequired(isAirQualityRequired)
            .Build();
        var requestUrl = WeatherUrlMapper.Build(weatherRequest);
        var request = new RestRequest(requestUrl);
        var response = await _client.ExecuteGetAsync(request);

        if (!response.IsSuccessful || response.Content is null)
        {
            // Log here
            return null;
        }

        var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(response.Content);
        return weatherResponse;
    }
}