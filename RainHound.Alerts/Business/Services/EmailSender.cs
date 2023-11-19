using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Entities;
using RainHound.Alerts.Models.FoundAlerts;
using Azure;
using Azure.Communication.Email;
using RainHound.Alerts.Configuration;

namespace RainHound.Alerts.Business.Services;

public class EmailSender : IEmailSender
{
    private readonly IEmailsTableStorageService _emailsTableStorageService;
    private readonly AlertsConfiguration _alertsConfiguration;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IEmailsTableStorageService emailsTableStorageService,
        AlertsConfiguration alertsConfiguration,
        ILogger<EmailSender> logger)
    {
        _emailsTableStorageService = emailsTableStorageService;
        _alertsConfiguration = alertsConfiguration;
        _logger = logger;
    }

    public async Task SendEmailToUserAsync(List<FoundAlertModel> foundAlerts)
    {
        var emailEntities = await GetUnsentEmailsAsync(foundAlerts);

        if (emailEntities.Count > 0)
        {
            await SendEmailAlertAsync(emailEntities);
        }
        else
        {
            _logger.LogInformation("All emails already sent.");
        }
    }

    private async Task<List<EmailEntity>> GetUnsentEmailsAsync(List<FoundAlertModel> foundAlerts)
    {
        var emailEntities = new List<EmailEntity>();

        foreach (var alert in foundAlerts)
        {
            var emailEntity = EmailEntityMapper.MapToEntity(alert);

            if (await _emailsTableStorageService.IsAlertEmailedAsync(emailEntity))
            {
                _logger.LogWarning("Already sent email to {Email} in city {City}. Skipping", alert.Email, alert.City);
                continue;
            }

            await _emailsTableStorageService.UpsertEmailAsync(emailEntity);
            emailEntities.Add(emailEntity);
        }

        return emailEntities;
    }

    private async Task SendEmailAlertAsync(List<EmailEntity> emailAlerts)
    {
        var emailClient = new EmailClient(_alertsConfiguration.ConnectionString);
        var emailMessage = FormatAlertEmail(emailAlerts);

        try
        {
            var firstEmail = emailAlerts.First();

            _logger.LogInformation("Sending email alert to {Email} for city {City} for date {StartDate}", firstEmail.Email, firstEmail.City, firstEmail.StartDate);
            await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            _logger.LogInformation("Email sent");
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not send the email alert, marking it as failed. Exception: <{Exception}>", ex.Message);
            await SetEmailsInErrorAsync(emailAlerts);
        }
    }

    private EmailMessage FormatAlertEmail(List<EmailEntity> emailAlerts)
    {
        var alertMessages = string.Join(Environment.NewLine, emailAlerts.Select(a =>
                $"\t{a.Description}, in {a.City}. Starts at {a.StartDate}, ends at {a.EndDate}."));
        var emailContent = new EmailContent(_alertsConfiguration.Subject)
        {
            PlainText = $"Alerts:{Environment.NewLine}{alertMessages}"
        };
        var emailMessage = new EmailMessage(_alertsConfiguration.FromEmail, emailAlerts.First().Email, emailContent);

        return emailMessage;
    }

    private async Task SetEmailsInErrorAsync(List<EmailEntity> emailAlerts)
    {
        var upsertTasks = new List<Task>();

        foreach (var emailAlert in emailAlerts)
        {
            emailAlert.IsInError = true;
            var upsertTask = _emailsTableStorageService.UpsertEmailAsync(emailAlert);
            upsertTasks.Add(upsertTask);
        }

        await Task.WhenAll(upsertTasks);
    }
}