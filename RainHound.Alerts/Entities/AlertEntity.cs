using Azure;
using Azure.Data.Tables;

namespace RainHound.Alerts.Entities;
public class AlertEntity : ITableEntity
{
    public double? MinTemp { get; set; }
    public double? MaxTemp { get; set; }
    public double? ChancesOfRain { get; set; }

    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
