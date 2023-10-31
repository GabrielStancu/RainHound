using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string email, List<FoundAlertModel> foundAlerts);
}