using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services;

public class EmailSender : IEmailSender
{
    private readonly IEmailsTableStorageService _emailsTableStorageService;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IEmailsTableStorageService emailsTableStorageService, ILogger<EmailSender> logger)
    {
        _emailsTableStorageService = emailsTableStorageService;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, List<FoundAlertModel> foundAlerts)
    {
        foreach (var alert in foundAlerts)
        {
            var emailEntity = EmailEntityMapper.MapToEntity(alert);

            if (await _emailsTableStorageService.IsAlertEmailedAsync(emailEntity))
            {
                _logger.LogWarning($"Already sent email to {alert.Email} in city {alert.City}, for {alert.Description}. Skipping");
                continue;
            }

            await _emailsTableStorageService.UpsertEmailAsync(emailEntity);
        }
    }
}
