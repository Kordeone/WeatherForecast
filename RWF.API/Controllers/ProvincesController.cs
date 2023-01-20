#region

using Microsoft.AspNetCore.Mvc;
using RWF.Logic;
using RWF.Model.DTOs;

#endregion

namespace RWF.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProvincesController : ControllerBase
{
    private readonly IProvinceLogic _logic;

    public ProvincesController(IProvinceLogic logic)
    {
        _logic = logic;
    }

    [HttpGet("Get")]
    public async Task<IResult> GetProvincesOnly()
    {
        var provinces = await _logic.GetSummary();
        return Results.Ok(provinces);
    }

    [HttpGet("AllProvincesWithForecast")]
    public async Task<IResult> AllProvincesWithForecast()
    {
        var provinces = await _logic.GetAllDetailed();
        return Results.Ok(provinces);
    }

    [HttpPost("Insert")]
    public async Task<IResult> InsertProvince(ProvinceDto param)
    {
        if (await _logic.Add(param))
        {
            return Results.Ok("Added Successfully");
        }

        return Results.BadRequest("Can't add province");
    }

    [HttpPost("InsertCities")]
    public async Task<IResult> InsertCityToProvince(CityDto param)
    {
        if (await _logic.AddCityToProvince(param))
        {
            return Results.Ok("Added Successfully");
        }

        return Results.BadRequest("Can't add province");
    }

    [HttpPut("Update/{id:int}")]
    public async Task<IResult> UpdateProvince(ProvinceDto param, int id)
    {
        if (await _logic.Update(param, id))
        {
            return Results.Ok("Updated Successfully");
        }

        return Results.BadRequest("Can't add province");
    }
}