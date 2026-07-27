namespace JustAnApi.Features.Weather;

internal static class WeatherForecastGenerator
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public static WeatherForecast Generate(DateOnly date) =>
        new(date, Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]);
}
