using JustAnApi.Features.Weather;

namespace JustAnApi.Tests.Weather;

public class GetWeatherForecastHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsFiveForecasts()
    {
        // Arrange
        var handler = new GetWeatherForecastHandler();

        // Act
        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        // Assert
        Assert.Equal(5, result.Length);
    }

    [Fact]
    public async Task Handle_ReturnsForecastsForTheNextFiveConsecutiveDays()
    {
        // Arrange
        var handler = new GetWeatherForecastHandler();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var expectedDates = Enumerable.Range(1, 5).Select(today.AddDays);

        // Act
        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        // Assert
        Assert.Equal(expectedDates, result.Select(forecast => forecast.Date));
    }

    [Fact]
    public async Task Handle_ReturnsForecastsWithTemperatureInExpectedRange()
    {
        // Arrange
        var handler = new GetWeatherForecastHandler();

        // Act
        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        // Assert
        Assert.All(result, forecast => Assert.InRange(forecast.TemperatureC, -20, 54));
    }

    [Fact]
    public async Task Handle_ReturnsForecastsWithNonEmptySummary()
    {
        // Arrange
        var handler = new GetWeatherForecastHandler();

        // Act
        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        // Assert
        Assert.All(result, forecast => Assert.False(string.IsNullOrWhiteSpace(forecast.Summary)));
    }
}
