using Com.Weather.Task1.Domain.Dto;

namespace Com.Weather.Task1.Domain.Services.Contracts
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetAsync(string rowKey, CancellationToken ct);

        Task<IEnumerable<WeatherResponseDto>> GetAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct);
    }
}
