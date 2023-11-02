using Azure.Data.Tables;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RainHound.Alerts.Business.Services;
using RainHound.Alerts.Business.Services.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, s) =>
    {
        s.AddHttpClient("WeatherApi", );
        s.AddScoped<IAlertsTableStorageService, AlertsTableStorageService>();
        s.AddScoped<IAlertsProcessor, AlertsProcessor>();
        s.AddScoped<IAlertsChecker, AlertsChecker>();
        s.AddScoped<IEmailSender, EmailSender>();
        s.AddScoped<IForecastService, ForecastService>();
        //
    })
    .Build();

host.Run();
