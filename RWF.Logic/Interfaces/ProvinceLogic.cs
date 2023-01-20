#region

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RWF.DataAccess;
using RWF.Model.DTOs;
using RWF.Model.Entities;

#endregion

namespace RWF.Logic.Interfaces;

public class ProvinceLogic : IProvinceLogic
{
    private readonly IGenericRepository<Province> _genericRepository;
    private readonly IMapper _mapper;


    public ProvinceLogic(IGenericRepository<Province> genericRepository, IMapper mapper)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Province>> GetAllDetailed()
    {
        return await _genericRepository.GetAllAsync(null,
            query => query.Include(province => province.Cities).ThenInclude(city => city.Forecast));
    }

    public async Task<IEnumerable<ProvinceInfoDto>> GetSummary()
    {
        var detailed = await _genericRepository.GetAllAsync();

        return detailed.Select(province => _mapper.Map<ProvinceInfoDto>(province)).ToList();
    }

    public async Task<bool> Add(ProvinceDto param)
    {
        if (await _genericRepository.AnyAsync(x => x.Name == param.Name)) return false;

        _genericRepository.Add(_mapper.Map<Province>(param));
        return true;
    }

    public async Task<bool> AddCityToProvince(CityDto param)
    {
        if (!await _genericRepository.AnyAsync(province => province.Id == param.ProvinceId)) return false;

        var savedProvince = await _genericRepository.GetAsync(param.ProvinceId,
            province => province.Include(province => province.Cities));

        if (savedProvince.Cities.Any(city => city.Name == param.Name)) return false;

        savedProvince.Cities.Add(new City
        {
            Name = param.Name
        });
        _genericRepository.Update(savedProvince);
        // await _genericRepository.SaveChanges();
        return true;
    }

    public async Task<bool> Update(ProvinceDto param, int id)
    {
        if (!await _genericRepository.AnyAsync(x => x.Id == id)) return false;
        _genericRepository.Update(_mapper.Map<Province>(param));
        return true;
    }
}