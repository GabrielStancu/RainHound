﻿using Azure.Data.Tables;
using Azure;

namespace RainHound.Alerts.Entities;

public class EmailEntity : ITableEntity
{
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsInError { get; set; }

    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
