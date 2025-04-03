using Azure;
using Azure.Data.Tables;

namespace Com.Weather.Task1.Domain.Entities
{
    public class WeatherRequestInfo : ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public bool IsSuccessful { get; set; }

        public string BlobName { get; set; }
    }
}
