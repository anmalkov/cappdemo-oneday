var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", async (IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("oneday");
    logger.LogInformation("Request received");

    try {

        var client = httpClientFactory.CreateClient();
        var forecast = await client.GetFromJsonAsync<List<WeatherForecast>>("http://localhost:3500/v1.0/invoke/forecast/method/forecast");

        logger.LogInformation($"Recerved {forecast?.Count} forecasts");

        return forecast?.First();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error occured");
        throw;
    }
})
.WithName("GetWeatherForecastForOneDay");

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}