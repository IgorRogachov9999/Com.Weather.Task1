using Com.Weather.Task1.Domain.Entities;
using Com.Weather.Task1.Domain.Helpers;
using Com.Weather.Task1.Domain.Services.Contracts;

namespace Com.Weather.Task1.Domain.Services
{
    public class WeatherPollerService : IWeatherPollerService
    {
        private readonly IOpenWeatherMapClientService _openWeatherMapClient;
        private readonly ITableService _tableService;
        private readonly IStorageService _storageService;

        public WeatherPollerService(
            IOpenWeatherMapClientService openWeatherMapClient,
            ITableService tableService,
            IStorageService storageService)
        {
            _openWeatherMapClient = openWeatherMapClient;
            _tableService = tableService;
            _storageService = storageService;
        }

        public async Task PollWeatherAsync(CancellationToken ct)
        {
            var timestamp = DateTimeOffset.UtcNow;
            var partitionKey = TableHelper.GetParitionKey(timestamp);
            var rowKey = TableHelper.GetRowKey(timestamp);

            var weatherData = await _openWeatherMapClient.GetWeatherDataAsync(ct);
            var blobName = weatherData != null
                ? TableHelper.GetBlobNameByRowKey(rowKey)
                : null;

            if (!string.IsNullOrEmpty(blobName))
            {
                await _storageService.SaveAsync(weatherData, blobName, ct);
            }

            await _tableService.SaveAsync(new WeatherRequestInfo()
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Timestamp = timestamp,
                BlobName = blobName,
                IsSuccessful = !string.IsNullOrEmpty(blobName)
            }, ct);
        }
    }
}
