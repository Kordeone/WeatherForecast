#region

using RWF.Model.DTOs;
using RWF.Model.Entities;

#endregion

namespace RWF.Logic;

public interface IProvinceLogic
{
    Task<IEnumerable<Province>> GetAllDetailed();

    Task<IEnumerable<ProvinceInfoDto>> GetSummary();
    Task<bool> Add(ProvinceDto param);
    Task<bool> AddCityToProvince(CityDto param);
    Task<bool> Update(ProvinceDto param, int id);
}