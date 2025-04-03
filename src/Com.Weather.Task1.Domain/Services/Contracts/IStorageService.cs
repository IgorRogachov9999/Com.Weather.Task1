using Com.Weather.Task1.Domain.Dto;

namespace Com.Weather.Task1.Domain.Services.Contracts
{
    public interface IStorageService
    {
        Task SaveAsync(WeatherResponseDto weatherResponseDto, string blobName, CancellationToken ct);

        Task<WeatherResponseDto> GetWeatherDataAsync(string blobName, CancellationToken ct);

        Task<IEnumerable<WeatherResponseDto>> GetWeatherDataAsync(string[] blobNames, CancellationToken ct);
    }
}
