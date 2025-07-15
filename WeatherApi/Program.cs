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

builder.Services.AddControllers();

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

app.MapControllers();

app.Run();

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
