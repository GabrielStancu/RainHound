using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Configuration;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services;

public class EmailsTableStorageService : IEmailsTableStorageService
{
    private readonly TableStorageConfiguration _configuration;
    private readonly ILogger<EmailsTableStorageService> _logger;
    private TableClient? _tableClient;
    private const string TableName = "Emails";

    public EmailsTableStorageService(TableStorageConfiguration configuration, ILogger<EmailsTableStorageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> IsAlertEmailedAsync(EmailEntity emailEntity)
    {
        await InitTableClientAsync();

        _logger.LogInformation($"Searching alerts for {JsonSerializer.Serialize(emailEntity)}");
        var tableEmails = _tableClient!
            .QueryAsync<EmailEntity>(e => 
                e.Email == emailEntity.Email && e.StartDate >= emailEntity.StartDate.AddHours(-1)
                                             && e.StartDate <= emailEntity.StartDate.AddHours(1)
                                             && e.City == emailEntity.City && !e.IsInError);

        await foreach (var emails in tableEmails.AsPages())
        {
            if (emails.Values.Count > 0)
            {
                _logger.LogInformation($"Found rows: {JsonSerializer.Serialize(emails.Values)}");
                return true;
            }
        }

        return false;
    }

    public async Task<Response> UpsertEmailAsync(EmailEntity emailEntity)
    {
        await InitTableClientAsync();

        _logger.LogInformation($"Upserting {JsonSerializer.Serialize(emailEntity)}");
        var response = await _tableClient!.UpsertEntityAsync(emailEntity);
        _logger.LogInformation($"Upserted {JsonSerializer.Serialize(emailEntity)}");

        return response;
    }

    private async Task InitTableClientAsync()
    {
        _logger.LogInformation($"Initializing table client for {TableName}");

        if (_tableClient != null)
        {
            _logger.LogInformation("Table already initialized, skipping...");
            return;
        }

        var serviceClient = new TableServiceClient(_configuration.ConnectionString);
        _tableClient = serviceClient.GetTableClient(TableName);
        await _tableClient.CreateIfNotExistsAsync();

        _logger.LogInformation($"Created table client for {TableName}");
    }
}
