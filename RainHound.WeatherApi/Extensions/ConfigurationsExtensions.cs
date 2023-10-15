namespace RainHound.WeatherApi.Extensions;

public static class ConfigurationsExtensions
{
    public static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName).Get<T>();

        if (section is null)
        {
            // LOG
            throw new ArgumentException("Section not configured!");
        }

        services.AddSingleton<T>(section);
    }
}
