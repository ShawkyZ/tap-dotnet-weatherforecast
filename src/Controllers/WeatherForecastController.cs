using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tanzu.WeatherForecast.Options;

namespace Tanzu.WeatherForecast.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IOptions<WeatherOptions> _weatherOptions;
    private readonly ILogger<WeatherForecastController> _logger;
    
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<WeatherOptions> weatherOptions)
    {
        _logger = logger;
        _weatherOptions = weatherOptions;
    }

    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    [HttpGet("option")]
    public string GetOptions()
    {
        return _weatherOptions.Value.Name;
    }
    
    [HttpGet]
    public IEnumerable<Models.WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Models.WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[index]
            })
            .ToArray();
    }
}
