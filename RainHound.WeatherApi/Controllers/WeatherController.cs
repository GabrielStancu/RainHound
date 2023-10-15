using Microsoft.AspNetCore.Mvc;
using RainHound.WeatherApi.Services;

namespace RainHound.WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("weather")]
    public async Task<IActionResult> GetWeatherForCity(string city, bool isAirQualityRequired)
    {
        var weatherResponse = await _weatherService.GetWeatherAsync(city, isAirQualityRequired);

        if (weatherResponse is null)
        {
            return BadRequest();
        }

        return Ok(weatherResponse);
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecastForCity(string city, int? days, bool aqi, bool alerts)
    {
        var forecastResponse = await _weatherService.GetForecastAsync(city, days ?? 1, aqi, alerts);

        if (forecastResponse is null)
        {
            return BadRequest();
        }

        return Ok(forecastResponse);
    }
}
