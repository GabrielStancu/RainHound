using Microsoft.AspNetCore.Mvc;

namespace RainHound.WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<TestController> _logger;

    public TestController(IWebHostEnvironment environment, ILogger<TestController> logger)
    {
        _environment = environment;
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
        string message = _environment.EnvironmentName;

        _logger.LogInformation(message);

        return Ok(message);
    }
}
