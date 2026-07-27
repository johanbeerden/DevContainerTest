using JustAnApi.Features.Weather;

namespace JustAnApi.Tests.Weather;

public class GetWeatherForecastByDayHandlerTests
{
    [Fact]
    public async Task Handle_WhenRequestedDayIsToday_ReturnsTodaysDate()
    {
        var handler = new GetWeatherForecastByDayHandler();
        var today = DateOnly.FromDateTime(DateTime.Now);

        var result = await handler.Handle(GetWeatherForecastByDayQuery.Create(today.DayOfWeek), CancellationToken.None);

        Assert.Equal(today, result.Date);
    }

    [Theory]
    [InlineData(DayOfWeek.Sunday)]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    public async Task Handle_ReturnsTheNextDateMatchingTheRequestedDayOfWeek(DayOfWeek dayOfWeek)
    {
        var handler = new GetWeatherForecastByDayHandler();
        var today = DateOnly.FromDateTime(DateTime.Now);

        var result = await handler.Handle(GetWeatherForecastByDayQuery.Create(dayOfWeek), CancellationToken.None);

        Assert.Equal(dayOfWeek, result.Date.DayOfWeek);
        Assert.InRange(result.Date.DayNumber - today.DayNumber, 0, 6);
    }

    [Fact]
    public async Task Handle_ReturnsForecastWithTemperatureInExpectedRange()
    {
        var handler = new GetWeatherForecastByDayHandler();

        var result = await handler.Handle(GetWeatherForecastByDayQuery.Create(DayOfWeek.Friday), CancellationToken.None);

        Assert.InRange(result.TemperatureC, -20, 54);
    }
}
