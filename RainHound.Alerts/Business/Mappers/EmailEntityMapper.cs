using RainHound.Alerts.Entities;
using RainHound.Alerts.Models.FoundAlerts;

namespace RainHound.Alerts.Business.Mappers;

public class EmailEntityMapper
{
    public static EmailEntity MapToEntity(FoundAlertModel model)
    {
        var entity = new EmailEntity
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

        entity.StartDate = DateTime.SpecifyKind(entity.StartDate, DateTimeKind.Utc);
        entity.EndDate = DateTime.SpecifyKind(entity.EndDate, DateTimeKind.Utc);

        return entity;
    }
}
