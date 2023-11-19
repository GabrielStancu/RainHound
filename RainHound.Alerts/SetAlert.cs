using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models;

namespace RainHound.Alerts;

public class SetAlert
{
    private readonly IAlertsTableStorageService _alertsTableStorageService;
    private readonly ILogger _logger;

    public SetAlert(ILoggerFactory loggerFactory, IAlertsTableStorageService alertsTableStorageService)
    {
        _alertsTableStorageService = alertsTableStorageService;
        _logger = loggerFactory.CreateLogger<SetAlert>();
    }

    [Function(nameof(SetAlert))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var content = await new StreamReader(req.Body).ReadToEndAsync();
        _logger.LogInformation("Received request <{SetAlertRequestContent}>", content);

        var setAlertRequest = JsonSerializer.Deserialize<AlertModel>(content);
        if (setAlertRequest is null)
        {
            _logger.LogError("Received invalid request!");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var alertEntity = AlertEntityMapper.MapToEntity(setAlertRequest);
        var response = await _alertsTableStorageService.UpsertAlertAsync(alertEntity);

        if (response.IsError)
        {
            _logger.LogError("Failed setting the alert with error: <{SetAlertErrorResponse}>", Encoding.ASCII.GetString(response.Content));
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        _logger.LogInformation("Successfully set the alert");
        return req.CreateResponse(HttpStatusCode.OK);
    }
}