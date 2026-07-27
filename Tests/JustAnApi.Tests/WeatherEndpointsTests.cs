using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using JustAnApi.Features.Weather;
using Microsoft.AspNetCore.Mvc.Testing;

namespace JustAnApi.Tests;

public class WeatherEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _client;

    public WeatherEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsOkWithFiveForecasts()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");
        var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>(JsonOptions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts!.Length);
    }

    [Theory]
    [InlineData("Monday")]
    [InlineData("monday")]
    [InlineData("MONDAY")]
    public async Task GetWeatherForecastByDay_ValidDayName_ReturnsOkWithMatchingForecast(string dayName)
    {
        // Act
        var response = await _client.GetAsync($"/weatherforecast/{dayName}");
        var forecast = await response.Content.ReadFromJsonAsync<WeatherForecast>(JsonOptions);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(forecast);
        Assert.Equal(DayOfWeek.Monday, forecast!.Date.DayOfWeek);
    }

    [Fact]
    public async Task GetWeatherForecastByDay_InvalidDayName_ReturnsBadRequestWithValidationError()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast/notaday");
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var dayOfWeekErrors = document.RootElement.GetProperty("errors").GetProperty("dayOfWeek");
        Assert.Contains("notaday", dayOfWeekErrors[0].GetString());
    }
}
