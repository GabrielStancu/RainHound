using RainHound.Alerts.Entities;
using RainHound.Alerts.Models;

namespace RainHound.Alerts.Business.Mappers;
public class AlertEntityMapper
{
    public static AlertEntity MapToEntity(AlertModel model) 
        => new()
        {
            Email = model.Email,
            MinTemp = model.MinTemp,
            MaxTemp = model.MaxTemp,
            ChancesOfRain = model.ChancesOfRain,
            City = model.City,
            PartitionKey = model.City,
            RowKey = Guid.NewGuid().ToString()
        };

    public static AlertModel MapToAlert(AlertEntity entity)
        => new()
        {
            Email = entity.Email,
            MinTemp = entity.MinTemp,
            MaxTemp = entity.MaxTemp,
            ChancesOfRain = entity.ChancesOfRain,
            City = entity.City,
        };
}
