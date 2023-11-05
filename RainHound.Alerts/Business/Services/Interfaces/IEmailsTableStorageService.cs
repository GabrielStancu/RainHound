using Azure;
using RainHound.Alerts.Entities;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IEmailsTableStorageService
{
    Task<bool> IsAlertEmailedAsync(EmailEntity emailEntity);
    Task<Response> UpsertEmailAsync(EmailEntity emailEntity);
}