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
        var response = await _client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>(JsonOptions);
        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts!.Length);
    }

    [Theory]
    [InlineData("Monday")]
    [InlineData("monday")]
    [InlineData("MONDAY")]
    public async Task GetWeatherForecastByDay_ValidDayName_ReturnsOkWithMatchingForecast(string dayName)
    {
        var response = await _client.GetAsync($"/weatherforecast/{dayName}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var forecast = await response.Content.ReadFromJsonAsync<WeatherForecast>(JsonOptions);
        Assert.NotNull(forecast);
        Assert.Equal(DayOfWeek.Monday, forecast!.Date.DayOfWeek);
    }

    [Fact]
    public async Task GetWeatherForecastByDay_InvalidDayName_ReturnsBadRequestWithValidationError()
    {
        var response = await _client.GetAsync("/weatherforecast/notaday");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var dayOfWeekErrors = document.RootElement.GetProperty("errors").GetProperty("dayOfWeek");
        Assert.Contains("notaday", dayOfWeekErrors[0].GetString());
    }
}
