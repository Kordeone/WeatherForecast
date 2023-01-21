#region

using RWF.Model.DTOs;

#endregion

namespace RWF.Logic.Interfaces;

public interface ICityLogic
{
    Task<IEnumerable<CityInfoDto>> GetAllDetailed();

    Task<IEnumerable<CityDto>> GetSummary();
    Task<CityInfoDto> GetWithId(int id);
    Task<CityInfoDto> GetCityForecast(string cityName);
    Task<bool> Add(CityInsertDto param);
    Task<bool> AddForecastToCity(WeatherForecastDto param);
    Task<bool> Update(CityDto param, int id);
}