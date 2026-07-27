using Mediator;

namespace JustAnApi.Features.Weather;

public sealed record GetWeatherForecastQuery : IRequest<WeatherForecast[]>;

public sealed class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecastQuery, WeatherForecast[]>
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public ValueTask<WeatherForecast[]> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var forecast = Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        return new ValueTask<WeatherForecast[]>(forecast);
    }
}
