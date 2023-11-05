using Azure.Data.Tables;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RainHound.Alerts.Configuration;

namespace RainHound.Alerts.Extensions;

public static class ServiceCollectionExtensions
{
    public static OptionsBuilder<T> AddConfiguration<T>(this IServiceCollection services, string sectionName, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : class
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (string.IsNullOrWhiteSpace(sectionName))
        {
            throw new ArgumentNullException(nameof(sectionName));
        }

        services.Add(new ServiceDescriptor(typeof(T), provider =>
        {
            var options = provider.GetRequiredService<IOptions<T>>();
            return options.Value;
        }, serviceLifetime));

        return services.AddOptions<T>().Configure<IConfiguration>((customSetting, configuration) =>
        {
            configuration.GetSection(sectionName).Bind(customSetting);
        });
    }

    public static void AddWeatherApiHttpClient(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var configuration = scope.ServiceProvider.GetService<WeatherApiConfiguration>();

        if (configuration is null || string.IsNullOrEmpty(configuration.Url))
        {
            throw new ArgumentNullException(nameof(WeatherApiConfiguration));
        }

        services.AddHttpClient(WeatherApiConfiguration.ClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.Url);
        });
    }

    public static void AddAlertsTableAzureClient(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var configuration = scope.ServiceProvider.GetService<AlertsStorageConfiguration>();

        if (configuration is null || string.IsNullOrEmpty(configuration.ConnectionString) || string.IsNullOrEmpty(configuration.TableStorage))
        {
            throw new ArgumentNullException(nameof(TableClient));
        }

        services.AddAzureClients(b =>
        {
            b.AddClient<TableClient, TableClientOptions>((_, _, _) =>
                new TableClient(configuration.ConnectionString, configuration.TableStorage));
        });
    }
}
