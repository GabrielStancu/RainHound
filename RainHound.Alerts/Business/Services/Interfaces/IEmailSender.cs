using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmailToUserAsync(List<FoundAlertModel> foundAlerts);
}