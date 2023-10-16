using Microsoft.AspNetCore.Mvc;
using RainHound.WeatherApi.Configuration;

namespace RainHound.WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly EnvironmentConfiguration _config;
    private readonly ILogger<TestController> _logger;

    public TestController(IWebHostEnvironment environment, EnvironmentConfiguration config, ILogger<TestController> logger)
    {
        _environment = environment;
        _config = config;
        _logger = logger;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        string message = "pong";

        _logger.LogInformation(message);

        return Ok(message);
    }

    [HttpGet("environment")]
    public IActionResult GetEnvironment()
    {
        string message = $"Environment variable: {_environment.EnvironmentName}. Configured value: {_config.Name}";

        _logger.LogInformation(message);

        return Ok(message);
    }
}
