using RainHound.WeatherApi.Business;
using RainHound.WeatherApi.Configuration;

namespace RainHound.WeatherApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName).Get<T>();

        if (section is null)
        {
            throw new ArgumentException($"Section {sectionName} not configured!");
        }

        services.AddSingleton(section);
    }

    public static void AddCorsPolicies(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        var clientConfiguration = scope.ServiceProvider.GetService<ClientConfiguration>();
        var alertsFunctionConfiguration = scope.ServiceProvider.GetService<AlertsFunctionConfiguration>();

        if (string.IsNullOrEmpty(clientConfiguration?.BaseUrl))
            throw new ArgumentException("Client configuration not set!");

        if (string.IsNullOrEmpty(alertsFunctionConfiguration?.BaseUrl))
            throw new ArgumentException("Alerts Function configuration not set!");


        services.AddCors(options =>
        {
            options.AddPolicy(name: Constants.Cors.ClientPolicy, policy =>
            {
                policy.WithOrigins(clientConfiguration.BaseUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            options.AddPolicy(name: Constants.Cors.AlertsFunctionPolicy, policy =>
            {
                policy.WithOrigins(alertsFunctionConfiguration.BaseUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
