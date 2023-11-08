using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RainHound.Alerts.Business.Services;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Configuration;
using RainHound.Alerts.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, s) =>
    {
        s.AddConfiguration<WeatherApiConfiguration>("WeatherApi");
        s.AddConfiguration<TableStorageConfiguration>("TableStorage");
        s.AddConfiguration<AlertsConfiguration>("Alerts");
        s.AddScoped<IAlertsProcessor, AlertsProcessor>();
        s.AddScoped<IAlertsChecker, AlertsChecker>();
        s.AddScoped<IEmailSender, EmailSender>();
        s.AddScoped<IForecastService, ForecastService>();
        s.AddScoped<IAlertsTableStorageService, AlertsTableStorageService>();
        s.AddScoped<IEmailsTableStorageService, EmailsTableStorageService>();
        s.AddWeatherApiHttpClient();
    })
    .Build();

host.Run();
