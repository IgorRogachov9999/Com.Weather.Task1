using Com.Weather.Task1.Domain.Entities;

namespace Com.Weather.Task1.Domain.Services.Contracts
{
    public interface ITableService
    {
        Task SaveAsync(WeatherRequestInfo requestInfo, CancellationToken ct);

        Task<WeatherRequestInfo> GetAsync(string partitionKey, string rowKey, CancellationToken ct);

        Task<IEnumerable<WeatherRequestInfo>> GetAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct);
    }
}
