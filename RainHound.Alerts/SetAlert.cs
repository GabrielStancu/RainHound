using System;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services;
using RainHound.Alerts.Requests;

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

        var content = await req.ReadAsStringAsync() ?? string.Empty;
        _logger.LogInformation($"Received request <{content}>");

        var setAlertRequest = JsonSerializer.Deserialize<SetAlertRequest>(content);
        if (setAlertRequest is null)
        {
            _logger.LogError("Received invalid request!");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var alertEntity = AlertEntityMapper.Map(setAlertRequest);
        var response = await _alertsTableStorageService.UpsertAlertAsync(alertEntity);

        if (response.IsError)
        {
            _logger.LogError($"Failed setting the alert with error: <{Encoding.ASCII.GetString(response.Content)}>");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        _logger.LogError("Successfully set the alert");
        return req.CreateResponse(HttpStatusCode.OK);
    }
}