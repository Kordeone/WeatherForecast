#region

#endregion

namespace RWF.Model.Entities;

public class City : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public ICollection<WeatherForecast> Forecast { get; set; } = null!;
}