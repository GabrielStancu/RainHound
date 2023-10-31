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
        s.AddScoped<IAlertsTableStorageService, AlertsTableStorageService>();
        s.AddScoped<IAlertsProcessor, AlertsProcessor>();
        s.AddScoped<IAlertsChecker, AlertsChecker>();
        s.AddScoped<IEmailSender, EmailSender>();
        s.AddScoped<IForecastService, ForecastService>();
        s.AddAzureClients(b =>
        {
            b.AddClient<TableClient, TableClientOptions>((_, _, _) => 
                new TableClient(ctx.Configuration["AlertsStorage:ConnectionString"],
                    ctx.Configuration["AlertsStorage:TableStorage"]));
        });
    })
    .Build();

host.Run();
