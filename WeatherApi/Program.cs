using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherApi.Options;
using WeatherDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var postgresOptions = new PostgresDbOptions();
builder.Configuration.GetSection("Db").Bind(postgresOptions);
var connectionString = PostgresDbOptions.GetConnectionString(postgresOptions);
Action<DbContextOptionsBuilder> configureDbContextPsql = ob => ob.UseNpgsql(connectionString);

builder.Services.AddDbContext<WeatherDbContext>(configureDbContextPsql);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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
.WithName("GetWeatherForecast");

app.MapPost("/weatherforecast", async (WeatherForecast requestBody, WeatherDb.WeatherDbContext context) =>
{
    context.WeatherForecasts.Add(new WeatherForecastEntity
    {
        Summary = requestBody.Summary,
        Date = requestBody.Date,
        TemperatureC = requestBody.TemperatureC
    });
    await context.SaveChangesAsync();
})
.WithName("PostWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
