using Mediator;

namespace JustAnApi.Features.Weather;

public sealed record GetWeatherForecastQuery : IRequest<WeatherForecast[]>
{
    public static GetWeatherForecastQuery Create() => new();
}

public sealed class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecastQuery, WeatherForecast[]>
{
    public ValueTask<WeatherForecast[]> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var forecast = Enumerable.Range(1, 5)
            .Select(index => WeatherForecastGenerator.Generate(DateOnly.FromDateTime(DateTime.Now.AddDays(index))))
            .ToArray();

        return new ValueTask<WeatherForecast[]>(forecast);
    }
}
