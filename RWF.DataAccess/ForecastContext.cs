#region

using Microsoft.EntityFrameworkCore;
using RWF.Model.Entities;

#endregion

namespace RWF.DataAccess;

public class ForecastContext : DbContext
{
    public ForecastContext(DbContextOptions<ForecastContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> Forecasts { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Province> Provinces { get; set; }
}