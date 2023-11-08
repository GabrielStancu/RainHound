using RainHound.Alerts.Entities;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Mappers;

public class EmailEntityMapper
{
    public static EmailEntity MapToEntity(FoundAlertModel model)
        => new()
        {
            Email = model.Email,
            City = model.City,
            StartDate = model.StartHour,
            EndDate = model.EndHour,
            Description = model.Description,
            PartitionKey = model.Email,
            RowKey = Guid.NewGuid().ToString(),
            IsInError = false
        };
}
