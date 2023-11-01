using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Configuration;

namespace RainHound.Alerts.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddWeatherApiHttpClient(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var configuration = scope.ServiceProvider.GetService<WeatherApiConfiguration>();

        if (configuration is null || string.IsNullOrEmpty(configuration.Url))
        {
            // Log error
            return;
        }

        services.AddHttpClient(WeatherApiConfiguration.ClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.Url);
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
        });
    }
}
