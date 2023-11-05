using Azure.Data.Tables;
using Azure;

namespace RainHound.Alerts.Entities;

public class EmailEntity : ITableEntity
{
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }

    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
