#region

using Microsoft.AspNetCore.Mvc;
using RWF.Logic.Interfaces;
using RWF.Model.DTOs;
using RWF.WebFramework.Hubs;

#endregion

namespace RWF.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly IForecastHub _forecastHub;
    private readonly ICityLogic _logic;

    public CitiesController(ICityLogic logic, IForecastHub forecastHub)
    {
        _logic = logic;
        _forecastHub = forecastHub;
    }

    [HttpGet("GetCityList")]
    public async Task<IResult> GetAllCities()
    {
        var cities = await _logic.GetSummary();
        return Results.Ok(cities);
    }

    [HttpGet("CitiesAndForecast")]
    public async Task<IResult> GetAllDetailed()
    {
        var cities = await _logic.GetAllDetailed();
        return Results.Ok(cities);
    }

    [HttpGet("SearchWithName")]
    public async Task<IResult> SearchCityWithName(string cityName)
    {
        var city = await _logic.GetCityForecast(cityName);
        return Results.Ok(city);
    }

    [HttpPost("Insert")]
    public async Task<IResult> InsertCity(CityInsertDto param)
    {
        if (await _logic.Add(param))
        {
            return Results.Ok("Added Successfully");
        }

        return Results.BadRequest("Can't add city");
    }

    [HttpPost("InsertForecast")]
    public async Task<IResult> AddForecastToCity(WeatherForecastDto param)
    {
        if (await _logic.AddForecastToCity(param))
        {
            return Results.Ok("Added Successfully");
        }

        return Results.BadRequest("Can't add forecast");
    }

    [HttpPut("Update/{id:int}")]
    public async Task<IResult> UpdateCity(CityDto param, int id)
    {
        if (await _logic.Update(param, id))
        {
            await _forecastHub.ChangedCity(await _logic.GetWithId(id));
            return Results.Ok("Updated Successfully");
        }

        return Results.BadRequest("Can't update city");
    }
}