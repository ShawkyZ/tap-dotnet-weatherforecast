namespace Tanzu.WeatherForecast.Tests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test_CallIndex_ReturnsCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html", response.Content.Headers.ContentType?.ToString());
    }
    
    
    [Fact]
    public async Task Test_CallWeatherForecastController_ReturnsCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }
    
    [Fact]
    public async Task Test_CallWeatherForecastController_ReturnsCorrectJsonResponse()
    {
        // Arrange
        var client = _factory.CreateClient();
        var summaries = new []{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        var expectedResult = Enumerable.Range(1, 5).Select(index => new Models.WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[index]
            }).OrderBy(x => x.Date).ToArray();
        
        // Act
        var response = await client.GetAsync("/WeatherForecast");
        var actualResult = (await response.Content.ReadFromJsonAsync<IEnumerable<Models.WeatherForecast>>() ?? Array.Empty<Models.WeatherForecast>())
            .OrderBy(x => x.Date).ToArray();
        
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        for (var i = 0; i < actualResult.Length; i++)
        {
            var actualWeatherForecast = actualResult[i];
            var expectedWeatherForecast = expectedResult[i];
            Assert.Equal(actualWeatherForecast.Summary, expectedWeatherForecast.Summary);
        }
    }
}