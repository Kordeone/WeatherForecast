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
    }
}