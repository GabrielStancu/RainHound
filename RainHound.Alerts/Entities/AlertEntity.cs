using Azure;
using Azure.Data.Tables;

namespace RainHound.Alerts.Entities;
public class AlertEntity : ITableEntity
{
    public string? Email { get; set; }
    public int? MinTemp { get; set; }
    public int? MaxTemp { get; set; }
    public int? ChancesOfRain { get; set; }

    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
