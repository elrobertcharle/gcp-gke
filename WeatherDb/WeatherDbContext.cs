using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDb
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions options) : base(options)
        {
            // dotnet ef migrations add InitialCreate --project WeatherDb --startup-project Weather
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecastEntity>().Property(e => e.Date).HasColumnType("date");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }
    }
}
