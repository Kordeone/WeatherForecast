#region

using Microsoft.AspNetCore.SignalR;
using RWF.Model.DTOs;

#endregion

namespace RWF.WebFramework.Hubs;

public interface IForecastHub
{
    Task OnConnectedAsync();
    Task ChangedCity(CityInfoDto cityInfo);
}

public class ForecastHub : Hub, IForecastHub
{
    private readonly IHubContext<ForecastHub> _hubContext;

    public ForecastHub(IHubContext<ForecastHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task ChangedCity(CityInfoDto cityInfo)
        => await _hubContext.Clients.All.SendAsync("ReceiveCityInfo", cityInfo);
}