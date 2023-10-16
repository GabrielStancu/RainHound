namespace RainHound.WeatherApi.Extensions;

public static class ConfigurationsExtensions
{
    public static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName).Get<T>();

        if (section is null)
        {
            throw new ArgumentException($"[G] Section {sectionName} not configured!");
        }

        services.AddSingleton<T>(section);
    }
}
