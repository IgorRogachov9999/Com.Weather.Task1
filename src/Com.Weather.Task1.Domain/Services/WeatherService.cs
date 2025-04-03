using Com.Weather.Task1.Domain.Dto;
using Com.Weather.Task1.Domain.Exceptions;
using Com.Weather.Task1.Domain.Extensions;
using Com.Weather.Task1.Domain.Helpers;
using Com.Weather.Task1.Domain.Services.Contracts;

namespace Com.Weather.Task1.Domain.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ITableService _tableService;
        private readonly IStorageService _storageService;

        public WeatherService(
            ITableService tableService,
            IStorageService storageService)
        {
            _tableService = tableService;
            _storageService = storageService;
        }

        public async Task<WeatherResponseDto> GetAsync(string rowKey, CancellationToken ct)
        {
            var partitionKey = TableHelper.GetParitionKey(rowKey);
            var requestInfo = await _tableService.GetAsync(partitionKey, rowKey, ct);

            if (requestInfo == null)
            {
                throw new EntityNotFoundException($"No data for {rowKey}.");
            }

            return await _storageService.GetWeatherDataAsync(requestInfo.BlobName, ct);
        }

        public async Task<IEnumerable<WeatherResponseDto>> GetAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
        {
            if (from >= to)
            {
                throw new InvalidArgumentException($"From date must no be grater or equal than To date.");
            }

            var requestsInfo = await _tableService.GetAsync(from, to, ct);

            if (requestsInfo.IsNullOrEmpty())
            {
                throw new EntityNotFoundException("No info for the passed timeframe.");
            }

            var blobNames = requestsInfo
                .Select(x => x.BlobName)
                .ToArray();
            return await _storageService.GetWeatherDataAsync(blobNames, ct);
        }
    }
}
