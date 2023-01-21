#region

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RWF.DataAccess;
using RWF.Logic.Interfaces;
using RWF.Model.DTOs;
using RWF.Model.Entities;

#endregion

namespace RWF.Logic;

public class CityLogic : ICityLogic
{
    private readonly IGenericRepository<City> _genericRepository;
    private readonly IMapper _mapper;


    public CityLogic(IGenericRepository<City> genericRepository, IMapper mapper)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CityInfoDto>> GetAllDetailed()
    {
        var allCities = await _genericRepository.GetAllAsync(includes: query => query.Include(city => city.Forecast));

        return allCities.Select(city => _mapper.Map<CityInfoDto>(city)).ToList();
    }

    public async Task<IEnumerable<CityDto>> GetSummary()
    {
        var detailed = await _genericRepository.GetAllAsync();

        return detailed.Select(province => _mapper.Map<CityDto>(province)).ToList();
    }

    public async Task<bool> Add(CityInsertDto param)
    {
        if (await _genericRepository.AnyAsync(x => x.Name == param.Name)) return false;

        _genericRepository.Add(_mapper.Map<City>(param));
        await _genericRepository.SaveChanges();
        return true;
    }


    public async Task<bool> Update(CityDto param, int id)
    {
        if (!await _genericRepository.AnyAsync(x => x.Id == id)) return false;
        var entity = _mapper.Map<City>(param);
        entity.Id = id;
        _genericRepository.Update(entity);
        await _genericRepository.SaveChanges();
        return true;
    }

    public async Task<bool> AddForecastToCity(WeatherForecastDto param)
    {
        if (!await _genericRepository.AnyAsync(city => city.Id == param.CityId)) return false;

        var savedCity = await _genericRepository.GetAsync(param.CityId,
            city => city.Include(city => city.Forecast));

        if (savedCity.Forecast.Any(forecast => forecast.Date.Day == param.Date.Day)) return false;

        savedCity.Forecast.Add(new WeatherForecast
        {
            Date = param.Date,
            WeekDay = param.WeekDay,
            MaxTemp = param.MaxTemp,
            MinTemp = param.MinTemp
        });
        _genericRepository.Update(savedCity);
        await _genericRepository.SaveChanges();
        return true;
    }

    public async Task<CityInfoDto> GetCityForecast(string cityName)
    {
        var city = await _genericRepository.GetAsync(city => city.Name.Contains(cityName),
            city => city.Include(city => city.Forecast));

        return _mapper.Map<CityInfoDto>(city);
    }

    public async Task<CityInfoDto> GetWithId(int id)
    {
        var city = await _genericRepository.GetAsync(id, cities => cities.Include(city => city.Forecast));
        return _mapper.Map<CityInfoDto>(city);
    }
}