using Com.Weather.Task1.Domain.Dto;

namespace Com.Weather.Task1.Domain.Services.Contracts
{
    public interface IOpenWeatherMapClientService
    {
        Task<WeatherResponseDto> GetWeatherDataAsync(CancellationToken ct);
    }
}
