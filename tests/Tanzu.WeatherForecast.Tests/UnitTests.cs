using Microsoft.Extensions.Options;
using Moq;
using Tanzu.WeatherForecast.Controllers;
using Tanzu.WeatherForecast.Options;

namespace Tanzu.WeatherForecast.Tests;

public class UnitTests
{
    private readonly ILogger<WeatherForecastController> _mockedLogger = new Mock<ILogger<WeatherForecastController>>().Object;
    private readonly IOptions<WeatherOptions> _mockedOptions = new Mock<IOptions<WeatherOptions>>().Object;

    [Fact]
    public void Test_CallGet_ReturnList()
    {
        // Arrange
        var weatherForecastController = new WeatherForecastController(_mockedLogger, _mockedOptions);
        var summaries = new[]{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        var expectedResult = Enumerable.Range(1, 5).Select(index => new Models.WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[index]
        }).OrderBy(x => x.Date).ToArray();
        
        // Act
        var actualResult = weatherForecastController.Get().ToArray();

        // Assert
        for (var i = 0; i < actualResult.Length; i++)
        {
            var actualWeatherForecast = actualResult[i];
            var expectedWeatherForecast = expectedResult[i];
            Assert.Equal(actualWeatherForecast.Summary, expectedWeatherForecast.Summary);
        }
    }
}