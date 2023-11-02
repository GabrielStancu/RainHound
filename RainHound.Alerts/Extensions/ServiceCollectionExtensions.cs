using Azure.Data.Tables;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
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

    public static void AddAzureClient(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var configuration = scope.ServiceProvider.GetService<AlertsStorageConfiguration>();

        if (configuration is null || string.IsNullOrEmpty(configuration.ConnectionString) || string.IsNullOrEmpty(configuration.TableStorage))
        {
            // Log error
            return;
        }

        services.AddAzureClients(b =>
        {
            b.AddClient<TableClient, TableClientOptions>((_, _, _) =>
                new TableClient(configuration.ConnectionString, configuration.TableStorage));
        });
    }
}
