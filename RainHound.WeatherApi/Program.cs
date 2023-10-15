using RainHound.WeatherApi.Configuration;
using RainHound.WeatherApi.Extensions;
using RainHound.WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddConfiguration<WeatherApiConfiguration>(builder.Configuration, WeatherApiConfiguration.SectionName);
var connstr = builder.Configuration.GetConnectionString("AppInsights");
builder.Logging.AddApplicationInsights(
    configureTelemetryConfiguration: (config) => 
        config.ConnectionString = builder.Configuration.GetConnectionString("AppInsights"),
        configureApplicationInsightsLoggerOptions: (options) => { }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
