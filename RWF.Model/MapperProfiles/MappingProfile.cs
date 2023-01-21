#region

using AutoMapper;
using RWF.Model.DTOs;
using RWF.Model.Entities;

#endregion

namespace RWF.Model.MapperProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Province, ProvinceDto>().ReverseMap();
        CreateMap<Province, ProvinceInfoDto>().ReverseMap();
        CreateMap<ProvinceInfoDto, ProvinceDto>().ReverseMap();

        CreateMap<City, CityDto>().ReverseMap();
        CreateMap<City, CityInfoDto>().ReverseMap();
        CreateMap<CityInfoDto, CityDto>().ReverseMap();

        CreateMap<WeatherForecast, WeatherForecastDto>().ReverseMap();
    }
}