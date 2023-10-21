using RainHound.WeatherApi.Configuration;
using RainHound.WeatherApi.Extensions;
using RainHound.WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowClient",
                      policy  =>
                      {
                        string clientUrl = builder.Configuration.GetSection("Client")["BaseUrl"]!;
                        policy.WithOrigins(clientUrl).AllowAnyHeader().AllowAnyMethod();
                      });
});
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddConfiguration<WeatherApiConfiguration>(builder.Configuration, WeatherApiConfiguration.SectionName);
builder.Services.AddConfiguration<EnvironmentConfiguration>(builder.Configuration, EnvironmentConfiguration.SectionName);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowClient");
app.UseAuthorization();
app.MapControllers();

app.Run();
