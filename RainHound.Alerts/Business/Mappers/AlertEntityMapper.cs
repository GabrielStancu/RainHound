using RainHound.Alerts.Entities;
using RainHound.Alerts.Models;

namespace RainHound.Alerts.Business.Mappers;
public class AlertEntityMapper
{
    public static AlertEntity MapToEntity(AlertModel model) 
        => new()
        {
            MinTemp = model.MinTemp,
            MaxTemp = model.MaxTemp,
            ChancesOfRain = model.ChancesOfRain,
            PartitionKey = model.City,
            RowKey = model.Email
        };

    public static AlertModel MapToAlert(AlertEntity entity)
        => new()
        {
            Email = entity.RowKey,
            MinTemp = entity.MinTemp,
            MaxTemp = entity.MaxTemp,
            ChancesOfRain = entity.ChancesOfRain,
            City = entity.PartitionKey,
        };
}
