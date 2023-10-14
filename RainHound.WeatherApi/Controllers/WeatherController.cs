using Microsoft.AspNetCore.Mvc;

namespace RainHound.WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    [HttpGet("weather/{city}/{days}")]
    public async Task<IActionResult> GetWeatherForCity(string city, int? days)
    {
        return Ok();
    }

    [HttpGet("forecast/{city}")]
    public async Task<IActionResult> GetForecastForCity(string city, int? days)
    {
        return Ok();
    }
}
