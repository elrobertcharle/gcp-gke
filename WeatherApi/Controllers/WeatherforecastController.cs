using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("weatherforecast")]
    public class WeatherforecastController : ControllerBase
    {
        private readonly WeatherDb.WeatherDbContext _context;
        public WeatherforecastController(WeatherDb.WeatherDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

            var forecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();

            return Ok(forecasts);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var item = await _context.WeatherForecasts.FindAsync(id, ct);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WeatherForecast requestBody, CancellationToken ct)
        {
            var item = new WeatherDb.WeatherForecastEntity
            {
                Date = requestBody.Date,
                Summary = requestBody.Summary,
                TemperatureC = requestBody.TemperatureC
            };
            _context.WeatherForecasts.Add(item);
            await _context.SaveChangesAsync(ct);

            return Ok(item.Id);
        }
    }
}
