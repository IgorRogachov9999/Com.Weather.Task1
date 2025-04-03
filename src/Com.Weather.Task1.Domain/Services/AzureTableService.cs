using Azure.Data.Tables;
using Com.Weather.Task1.Domain.Entities;
using Com.Weather.Task1.Domain.Factories.Contracts;
using Com.Weather.Task1.Domain.Helpers;
using Com.Weather.Task1.Domain.Options;
using Com.Weather.Task1.Domain.Services.Contracts;
using Microsoft.Extensions.Options;

namespace Com.Weather.Task1.Domain.Services
{
    public class AzureTableService : ITableService
    {
        private const int BatchSize = 12;
        private readonly TableClient _client;

        public AzureTableService(IStorageClientFactory clientFactory, IOptionsMonitor<WeatherStorageOptions> options)
        {
            _client = clientFactory.GetTableServiceClient().GetTableClient(options.CurrentValue.TableName);
            _client.CreateIfNotExists();
        }

        public Task SaveAsync(WeatherRequestInfo requestInfo, CancellationToken ct)
        {
            return _client.AddEntityAsync(requestInfo, ct);
        }

        public async Task<WeatherRequestInfo> GetAsync(string partitionKey, string rowKey, CancellationToken ct)
        {
            var response = await _client.GetEntityIfExistsAsync<WeatherRequestInfo>(partitionKey, rowKey, cancellationToken: ct);
            return response?.Value;
        }

        public async Task<IEnumerable<WeatherRequestInfo>> GetAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
        {
            var partitions = GetPartitions(from, to);
            var requests = new List<WeatherRequestInfo>();
            var pageCount = partitions.Count / BatchSize;

            for (int i = 0; i <= pageCount; i++)
            {
                var partitionsToRead = partitions.Skip(i * BatchSize).Take(BatchSize).ToArray();
                var getWeatherRequestInfoTasks = new List<Task<IEnumerable<WeatherRequestInfo>>>();

                foreach (var partition in partitionsToRead)
                {
                    getWeatherRequestInfoTasks.Add(GetAsync(partition, from, to, ct));
                }

                await Task.WhenAll(getWeatherRequestInfoTasks);
                requests.AddRange(getWeatherRequestInfoTasks.SelectMany(x => x.Result).ToArray());
            }

            return requests;
        }

        private async Task<IEnumerable<WeatherRequestInfo>> GetAsync(string partition, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
        {
            string filter = TableClient.CreateQueryFilter<WeatherRequestInfo>(
                         e => e.PartitionKey == partition && e.Timestamp >= from && e.Timestamp <= to && e.IsSuccessful);
            var requests = new List<WeatherRequestInfo>();

            await foreach (var entity in _client.QueryAsync<WeatherRequestInfo>(filter, cancellationToken: ct))
            {
                requests.Add(entity);
            }

            return requests;
        }

        private List<string> GetPartitions(DateTimeOffset from, DateTimeOffset to)
        {
            var partitions = new List<string>();
            var current = new DateTimeOffset(from.Year, from.Month, 1, 0, 0, 0, TimeSpan.Zero);

            while (current <= to)
            {
                partitions.Add(TableHelper.GetParitionKey(current));
                current.AddMonths(1);
            }

            return partitions;
        }
    }
}
