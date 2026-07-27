using Mediator;

namespace JustAnApi.Features.Weather;

public sealed record GetWeatherForecastByDayQuery(DayOfWeek DayOfWeek) : IRequest<WeatherForecast>;

public sealed class GetWeatherForecastByDayHandler : IRequestHandler<GetWeatherForecastByDayQuery, WeatherForecast>
{
    public ValueTask<WeatherForecast> Handle(GetWeatherForecastByDayQuery request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var daysUntil = ((int)request.DayOfWeek - (int)today.DayOfWeek + 7) % 7;
        var targetDate = today.AddDays(daysUntil);

        return new ValueTask<WeatherForecast>(WeatherForecastGenerator.Generate(targetDate));
    }
}
