﻿using Microsoft.Extensions.Logging;
using RainHound.Alerts.Business.Mappers;
using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Entities;
using RainHound.Alerts.Models.FoundAlerts;
using System.Net.Mail;
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
            await SendEmailAlertAsync(emailEntity);
        }
    }

    private async Task SendEmailAlertAsync(EmailEntity emailAlert)
    {
        var message = new MailMessage(_alertsConfiguration.FromEmail!, emailAlert.Email!);
        message.Subject = "Weather Alert";
        message.Body = $"Alert: {emailAlert.Description}, in {emailAlert.City}. Starts at {emailAlert.StartDate}, ends at {emailAlert.EndDate}.";
        var client = new SmtpClient("smtp.gmail.com");
        client.UseDefaultCredentials = true;

        try
        {
            await Task.Run(() => client.Send(message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not send the email alert, marking it as failed. Exception: <{ex}>");
            emailAlert.IsInError = true;
            await _emailsTableStorageService.UpsertEmailAsync(emailAlert);
        }
    }
}
