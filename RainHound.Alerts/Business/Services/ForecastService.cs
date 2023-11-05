using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Configuration;
using RainHound.Alerts.Models;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RainHound.Alerts.Business.Services;
public class ForecastService : IForecastService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ForecastService> _logger;

    public ForecastService(IHttpClientFactory clientFactory, ILogger<ForecastService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<ForecastResponse?> GetForecastAsync(ForecastRequestModel forecastRequest)
    {
        var client = _clientFactory.CreateClient(WeatherApiConfiguration.ClientName);
        var forecastUri = ForecastRequestUri(forecastRequest);
        var response = await client.GetAsync(forecastUri);
        ForecastResponse? forecastResponse = null;

        if (response.IsSuccessStatusCode)
        {
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            forecastResponse = await JsonSerializer.DeserializeAsync<ForecastResponse>(contentStream);
        }

        return forecastResponse;
    }

    private string ForecastRequestUri(ForecastRequestModel request) =>
        $"api/Weather/forecast?city={request.City}&aqi={request.Aqi}&days={request.Days}";
}
