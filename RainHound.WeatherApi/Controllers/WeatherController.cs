using Microsoft.AspNetCore.Mvc;
using RainHound.WeatherApi.Services;

namespace RainHound.WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("weather")]
    public async Task<IActionResult> GetWeatherForCity(string city, bool aqi)
    {
        _logger.LogInformation($"Received request at {nameof(GetWeatherForCity)}, for city {city}, aqi: {aqi}");

        var weatherResponse = await _weatherService.GetWeatherAsync(city, aqi);

        if (weatherResponse is null)
        {
            _logger.LogError($"Failed to get weather for city {city}, aqi: {aqi}");
            return BadRequest();
        }

        _logger.LogInformation($"Successfully got weather for city {city}, aqi: {aqi}");
        return Ok(weatherResponse);
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecastForCity(string city, int? days, bool aqi, bool alerts)
    {
        _logger.LogInformation($"Received request at {nameof(GetWeatherForCity)}, for city {city}, days: {days}, aqi: {aqi}, alerts: {alerts}");

        var forecastResponse = await _weatherService.GetForecastAsync(city, days ?? 1, aqi, alerts);

        if (forecastResponse is null)
        {
            _logger.LogError($"Failed to get weather for city {city}, days: {days}, aqi: {aqi}, alerts: {alerts}");
            return BadRequest();
        }

        _logger.LogInformation($"Successfully got weather for city {city}, days: {days}, aqi: {aqi}, alerts: {alerts}");
        return Ok(forecastResponse);
    }
}
