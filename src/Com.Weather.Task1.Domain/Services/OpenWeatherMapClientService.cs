using Com.Weather.Task1.Domain.Dto;
using Com.Weather.Task1.Domain.Options;
using Com.Weather.Task1.Domain.Services.Contracts;
using Microsoft.Extensions.Options;
using Polly;
using System.Text.Json;

namespace Com.Weather.Task1.Domain.Services
{
    public class OpenWeatherMapClientService : IOpenWeatherMapClientService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapOptions _openWeatherMapOptions;

        public OpenWeatherMapClientService(IHttpClientFactory factory, IOptionsMonitor<OpenWeatherMapOptions> options)
        {
            _openWeatherMapOptions = options.CurrentValue;
            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri(_openWeatherMapOptions.BaseUrl);
        }

        public async Task<WeatherResponseDto> GetWeatherDataAsync(CancellationToken ct)
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: x => TimeSpan.FromSeconds(Math.Pow(1, x)));

            var response = await policy.ExecuteAsync(() =>
                _httpClient.GetAsync($"data/2.5/weather?q=London&appid={_openWeatherMapOptions.ApiKey}", ct));

            return await GetWeatherResponse(response, ct);
        }

        private async Task<WeatherResponseDto> GetWeatherResponse(HttpResponseMessage response, CancellationToken ct)
        {
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<WeatherResponseDto>(json);
        }
    }
}
