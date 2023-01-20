#region

using RWF.DataAccess;
using RWF.Model.Entities;

#endregion

namespace RWF.Logic;

public class DataSeeder
{
    private readonly IGenericRepository<City> _citiesRepo;
    private readonly IGenericRepository<WeatherForecast> _forecastRepo;
    private readonly IGenericRepository<Province> _proviceRepo;

    public DataSeeder(IGenericRepository<Province> proviceRepo, IGenericRepository<WeatherForecast> forecastRepo,
        IGenericRepository<City> citiesRepo)
    {
        _proviceRepo = proviceRepo;
        _forecastRepo = forecastRepo;
        _citiesRepo = citiesRepo;
    }

    public void Seed()
    {
        var cities = new List<City>
        {
            new City
            {
                Name = "Babol",
                Forecast = GetFakeForecast()
            },
            new City
            {
                Name = "Sari",
                Forecast = GetFakeForecast()
            },
            new City
            {
                Name = "Qaemshahr",
                Forecast = GetFakeForecast()
            },
            new City
            {
                Name = "Amol",
                Forecast = GetFakeForecast()
            }
        };

        Province province = new()
        {
            Name = "Mazandaran",
            Capital = "Sari",
            Code = 011,
            Overall = 43.2m,
            CityCount = 65,
            Cities = cities
        };

        _proviceRepo.Add(province);
        _proviceRepo.SaveChanges();
    }


    private List<WeatherForecast> GetFakeForecast()
    {
        Random random = new();
        var forecasts = new List<WeatherForecast>();
        for (int i = 0; i < 10; i++)
        {
            forecasts.Add(new WeatherForecast
            {
                Date = DateTime.Now.AddDays(i),
                MaxTemp = random.Next(15, 30),
                MinTemp = random.Next(-5, 15),
                WeekDay = DateTime.Now.AddDays(i).DayOfWeek.ToString()
            });
        }

        return forecasts;
    }
}