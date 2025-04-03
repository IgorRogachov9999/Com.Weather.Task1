namespace Com.Weather.Task1.Domain.Services.Contracts
{
    public interface IWeatherPollerService
    {
        Task PollWeatherAsync(CancellationToken ct);
    }
}
