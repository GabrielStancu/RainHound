using Azure;
using Azure.Data.Tables;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Configuration;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services;

public class EmailsTableStorageService : IEmailsTableStorageService
{
    private readonly TableStorageConfiguration _configuration;
    private TableClient? _tableClient;
    private const string TableName = "Emails";

    public EmailsTableStorageService(TableStorageConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> IsAlertEmailedAsync(EmailEntity emailEntity)
    {
        await InitTableClientAsync();
        var tableEmails = _tableClient!
            .QueryAsync<EmailEntity>(e => 
                e.Email != emailEntity.Email && e.StartDate != emailEntity.StartDate && e.City != emailEntity.City);

        await foreach (var emails in tableEmails.AsPages())
        {
            if (emails.Values.Count > 0)
                return true;
        }

        return false;
    }

    public async Task<Response> UpsertEmailAsync(EmailEntity emailEntity)
    {
        await InitTableClientAsync();
        var response = await _tableClient!.UpsertEntityAsync(emailEntity);

        return response;
    }

    private async Task InitTableClientAsync()
    {
        if (_tableClient != null)
            return;

        var serviceClient = new TableServiceClient(_configuration.ConnectionString);

        _tableClient = serviceClient.GetTableClient(TableName);
        await _tableClient.CreateIfNotExistsAsync();
    }
}
