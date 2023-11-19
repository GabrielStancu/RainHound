using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using RainHound.WeatherApi.Business.RequestBuilders;
using RainHound.WeatherApi.Business.UrlBuilders;
using RainHound.WeatherApi.Business.UrlMappers;
using RainHound.WeatherApi.Configuration;
using RainHound.WeatherApi.Contracts.Requests;
using RainHound.WeatherApi.Contracts.Responses;
using RestSharp;

namespace RainHound.WeatherApi.Services;

public class WeatherService : IWeatherService
{
    private readonly WeatherApiConfiguration _weatherApiConfiguration;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WeatherService> _logger;
    private readonly RestClient _client;

    public WeatherService(WeatherApiConfiguration weatherApiConfiguration, IMemoryCache cache, ILogger<WeatherService> logger)
    {
        _weatherApiConfiguration = weatherApiConfiguration;
        _cache = cache;
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

        _logger.LogInformation("Getting forecast for request <{ForecastRequest}>", JsonSerializer.Serialize(forecastRequest));

        var cacheKey = $"Forecast_{DateTime.UtcNow.Date}_{DateTime.UtcNow.Hour}_{city}_{days}_{isAirQualityRequired}_{areAlertsRequired}";
        if (_cache.TryGetValue(cacheKey, out ForecastResponse? forecastResponse))
        {
            _logger.LogInformation("Obtained forecast from cache");
            return forecastResponse;
        }

        forecastResponse = await GetForecastFromApiAsync(forecastRequest);
        if (forecastResponse != null)
        {
            var absoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_weatherApiConfiguration.CacheDurationMinutes);
            _cache.Set(cacheKey, forecastResponse, absoluteExpiration);
            _logger.LogInformation("Set forecast in cache until {AbsoluteExpiration}", absoluteExpiration);
        }

        return forecastResponse;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string city, bool isAirQualityRequired)
    {
        var weatherRequest = new WeatherRequestBuilder(_weatherApiConfiguration.ApiKey, city)
            .IsAirQualityRequired(isAirQualityRequired)
            .Build();

        _logger.LogInformation("Getting weather for request <{WeatherRequest}>", JsonSerializer.Serialize(weatherRequest));

        var cacheKey = $"Forecast_{DateTime.UtcNow.Date}_{DateTime.UtcNow.Hour}_{city}_{isAirQualityRequired}";
        if (_cache.TryGetValue(cacheKey, out WeatherResponse? weatherResponse))
        {
            _logger.LogInformation("Obtained weather from cache");
            return weatherResponse;
        }

        weatherResponse = await GetWeatherFromApiAsync(weatherRequest);
        if (weatherResponse != null)
        {
            var absoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_weatherApiConfiguration.CacheDurationMinutes);
            _cache.Set(cacheKey, weatherResponse, absoluteExpiration);
            _logger.LogInformation("Set weather in cache until {AbsoluteExpiration}", absoluteExpiration);
        }

        return weatherResponse;
    }

    private async Task<ForecastResponse?> GetForecastFromApiAsync(ForecastRequest forecastRequest)
    {
        var requestUrl = ForecastUrlMapper.Build(forecastRequest);
        var request = new RestRequest(requestUrl);
        var response = await _client.ExecuteGetAsync(request);

        if (!response.IsSuccessful || response.Content is null)
        {
            _logger.LogError("Failed to get the forecast. Response: {ForecastErrorResponse}", response.Content);
            return null;
        }

        _logger.LogInformation("Received response: {ForecastSuccessResponse}", response.Content);

        return JsonSerializer.Deserialize<ForecastResponse>(response.Content);
    }

    private async Task<WeatherResponse?> GetWeatherFromApiAsync(WeatherRequest weatherRequest)
    {
        var requestUrl = WeatherUrlMapper.Build(weatherRequest);
        var request = new RestRequest(requestUrl);
        var response = await _client.ExecuteGetAsync(request);

        if (!response.IsSuccessful || response.Content is null)
        {
            _logger.LogError("Failed to get the weather. Response: {WeatherErrorResponse}", response.Content);
            return null;
        }

        _logger.LogInformation("Received response: {WeatherSuccessResponse}", response.Content);

        return JsonSerializer.Deserialize<WeatherResponse>(response.Content);
    }
}