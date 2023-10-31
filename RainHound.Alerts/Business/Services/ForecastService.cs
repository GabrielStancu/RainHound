using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models;

namespace RainHound.Alerts.Business.Services;
public class ForecastService : IForecastService
{
    public ForecastService() // inject HttpClient, configs to call the api
    {
        
    }

    public async Task<ForecastResponse> GetForecastAsync(ForecastRequestModel forecastRequest)
    {
        var response = new ForecastResponse();

        await Task.Delay(10);

        // call the api, return that response

        return response;
    }
}
