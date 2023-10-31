using RainHound.Alerts.Entities;
using RainHound.Alerts.Requests;

namespace RainHound.Alerts.Business.Mappers;
public class AlertEntityMapper
{
    public static AlertEntity Map(SetAlertRequest request) 
        => new AlertEntity
        {
            Email = request.Email,
            MinTemp = request.MinTemp,
            MaxTemp = request.MaxTemp,
            ChancesOfRain = request.ChancesOfRain,
            PartitionKey = request.Email,
            RowKey = Guid.NewGuid().ToString()
        };
}
