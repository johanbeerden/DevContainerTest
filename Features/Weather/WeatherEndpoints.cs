using System.Text.Json;
using Mediator;

namespace JustAnApi.Features.Weather;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", async (IMediator mediator) => await mediator.Send(new GetWeatherForecastQuery()))
            .WithName("GetWeatherForecast")
            .WithTags("Weather")
            .WithSummary("Get a 5-day weather forecast")
            .WithDescription("""
                Generates a random weather forecast for each of the next 5 days.
                Takes no input parameters. Each entry includes the date, temperature
                in Celsius and Fahrenheit, and a short text summary.
                """)
            .Produces<WeatherForecast[]>(StatusCodes.Status200OK, "application/json")
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                if (operation!.Responses is { } responses &&
                    responses.TryGetValue("200", out var response) &&
                    response.Content is { } content &&
                    content.TryGetValue("application/json", out var mediaType))
                {
                    mediaType.Example = JsonSerializer.SerializeToNode(new WeatherForecast[]
                    {
                        new(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), 25, "Warm"),
                        new(DateOnly.FromDateTime(DateTime.Now.AddDays(2)), -3, "Chilly"),
                        new(DateOnly.FromDateTime(DateTime.Now.AddDays(3)), 40, "Scorching"),
                        new(DateOnly.FromDateTime(DateTime.Now.AddDays(4)), 12, "Mild"),
                        new(DateOnly.FromDateTime(DateTime.Now.AddDays(5)), -8, "Freezing"),
                    });
                }

                return Task.CompletedTask;
            });

        return app;
    }
}
