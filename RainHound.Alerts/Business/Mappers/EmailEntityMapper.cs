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
            Date = DateTime.UtcNow.Date,
            Description = model.Description,
            PartitionKey = model.Email,
            RowKey = Guid.NewGuid().ToString()
        };
}
