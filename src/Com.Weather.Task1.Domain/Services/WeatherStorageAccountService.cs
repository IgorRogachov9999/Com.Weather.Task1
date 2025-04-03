using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Com.Weather.Task1.Domain.Dto;
using Com.Weather.Task1.Domain.Exceptions;
using Com.Weather.Task1.Domain.Factories.Contracts;
using Com.Weather.Task1.Domain.Options;
using Com.Weather.Task1.Domain.Services.Contracts;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Com.Weather.Task1.Domain.Services
{
    public class WeatherStorageAccountService : IStorageService
    {
        private const int BatchSize = 50;
        private readonly BlobContainerClient _client;

        public WeatherStorageAccountService(IStorageClientFactory clientFactory, IOptionsMonitor<WeatherStorageOptions> options)
        {
            _client = clientFactory.GetBlobServiceClient().GetBlobContainerClient(options.CurrentValue.BlobName);
            _client.CreateIfNotExists();
        }

        public async Task SaveAsync(WeatherResponseDto weatherResponseDto, string blobName, CancellationToken ct)
        {
            var blobClient = _client.GetAppendBlobClient(blobName);
            blobClient.CreateIfNotExists();
            var data = JsonSerializer.Serialize(weatherResponseDto);
            
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(data);
                writer.Flush();
                stream.Position = 0;
                await blobClient.AppendBlockAsync(stream, cancellationToken: ct);
            }
        }

        public async Task<WeatherResponseDto> GetWeatherDataAsync(string blobName, CancellationToken ct)
        {
            var blobClient = _client.GetBlobClient(blobName);

            if (!blobClient.Exists())
            {
                throw new EntityNotFoundException($"Blob with name {blobName} not found.");
            }

            using var data = await blobClient.OpenReadAsync(cancellationToken: ct);
            return await JsonSerializer.DeserializeAsync<WeatherResponseDto>(data, cancellationToken: ct);
        }

        public async Task<IEnumerable<WeatherResponseDto>> GetWeatherDataAsync(string[] blobNames, CancellationToken ct)
        {
            var weatherData = new List<WeatherResponseDto>();
            var pageCount = blobNames.Length / BatchSize;

            for (int i = 0; i <= pageCount; i++)
            {
                var blobsToRead = blobNames.Skip(i * BatchSize).Take(BatchSize).ToArray();
                var getWeatherDataTasks = new List<Task<WeatherResponseDto>>();

                foreach (var blobName in blobsToRead)
                {
                    getWeatherDataTasks.Add(GetWeatherDataAsync(blobName, ct));
                }

                await Task.WhenAll(getWeatherDataTasks);
                weatherData.AddRange(getWeatherDataTasks.Select(x => x.Result).ToArray());
            }

            return weatherData;
        }
    }
}
