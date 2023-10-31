using RainHound.Alerts.Business.Services.Interfaces;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Services;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, List<FoundAlertModel> foundAlerts)
    {
        await Task.Delay(10);
    }
}
