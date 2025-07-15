using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

app.MapGet("/weatherforecast/{id:int}", async ([FromRoute] int id, WeatherDb.WeatherDbContext context) =>
{
    var item = await context.WeatherForecasts.FirstOrDefaultAsync(x => x.Id == id);
    if (item == null)
        return Results.NotFound();

    return Results.Ok(item);
})
.WithName("GetWeatherForecastById");

app.MapPost("/weatherforecast", async (WeatherForecast requestBody, WeatherDb.WeatherDbContext context) =>
{
    var item = new WeatherForecastEntity
    {
        Summary = requestBody.Summary,
        Date = requestBody.Date,
        TemperatureC = requestBody.TemperatureC
    };
    context.WeatherForecasts.Add(item);
    await context.SaveChangesAsync();

    return Results.Ok(item.Id);
})
.WithName("PostWeatherForecast");

app.MapGet("/healthz", () => Results.Ok("Healthy"));

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
