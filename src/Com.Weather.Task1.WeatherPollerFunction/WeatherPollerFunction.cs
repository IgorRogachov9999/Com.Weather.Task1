using Com.Weather.Task1.Domain.Services.Contracts;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Com.Weather.Task1.WeatherPollerFunction
{
    public class WeatherPollerFunction
    {
        private readonly IWeatherPollerService _weatherPollerService;
        public WeatherPollerFunction(IWeatherPollerService weatherPollerService)
        {
            _weatherPollerService = weatherPollerService;
        }

        [FunctionName("WeatherPollerFunction")]
        public async Task Run([TimerTrigger("0 * * * * * ")]TimerInfo timer, ILogger log)
        {
            await _weatherPollerService.PollWeatherAsync(CancellationToken.None);
        }
    }
}
