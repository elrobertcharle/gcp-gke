namespace WeatherDb
{
    public class WeatherForecastEntity
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
