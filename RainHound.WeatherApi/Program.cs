using Microsoft.Extensions.Caching.Memory;
using RainHound.WeatherApi.Business;
using RainHound.WeatherApi.Configuration;
using RainHound.WeatherApi.Extensions;
using RainHound.WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddConfiguration<WeatherApiConfiguration>(builder.Configuration, WeatherApiConfiguration.SectionName);
builder.Services.AddConfiguration<EnvironmentConfiguration>(builder.Configuration, EnvironmentConfiguration.SectionName);
builder.Services.AddConfiguration<ClientConfiguration>(builder.Configuration, ClientConfiguration.SectionName);
builder.Services.AddConfiguration<AlertsFunctionConfiguration>(builder.Configuration, AlertsFunctionConfiguration.SectionName);
builder.Services.AddCorsPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(Constants.Cors.ClientPolicy);
app.UseCors(Constants.Cors.AlertsFunctionPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
