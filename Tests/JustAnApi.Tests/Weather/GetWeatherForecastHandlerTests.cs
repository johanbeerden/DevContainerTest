using JustAnApi.Features.Weather;

namespace JustAnApi.Tests.Weather;

public class GetWeatherForecastHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsFiveForecasts()
    {
        var handler = new GetWeatherForecastHandler();

        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        Assert.Equal(5, result.Length);
    }

    [Fact]
    public async Task Handle_ReturnsForecastsForTheNextFiveConsecutiveDays()
    {
        var handler = new GetWeatherForecastHandler();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var expectedDates = Enumerable.Range(1, 5).Select(today.AddDays);

        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        Assert.Equal(expectedDates, result.Select(forecast => forecast.Date));
    }

    [Fact]
    public async Task Handle_ReturnsForecastsWithTemperatureInExpectedRange()
    {
        var handler = new GetWeatherForecastHandler();

        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        Assert.All(result, forecast => Assert.InRange(forecast.TemperatureC, -20, 54));
    }

    [Fact]
    public async Task Handle_ReturnsForecastsWithNonEmptySummary()
    {
        var handler = new GetWeatherForecastHandler();

        var result = await handler.Handle(GetWeatherForecastQuery.Create(), CancellationToken.None);

        Assert.All(result, forecast => Assert.False(string.IsNullOrWhiteSpace(forecast.Summary)));
    }
}
