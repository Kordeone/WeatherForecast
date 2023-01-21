#region

using RWF.Model.Entities;

#endregion

namespace RWF.Model.DTOs;

public class CityInfoDto : CityDto
{
    public List<WeatherForecast> Forecast { get; set; }
}