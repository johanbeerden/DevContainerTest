using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "JustAnApi";
        document.Info.Version = "v1";
        document.Info.Description = "A small sample API that returns randomly generated weather forecasts.";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "JustAnApi v1");
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
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

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}