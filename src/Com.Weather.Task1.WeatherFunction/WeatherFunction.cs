using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Com.Weather.Task1.Domain.Services.Contracts;
using System.Threading;
using System;
using Com.Weather.Task1.WeatherFunction.Infrastructure;

namespace Com.Weather.Task1.WeatherFunction
{
    public class WeatherFunction : FunctionBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherFunction(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [FunctionName("GetWeather")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/v1/weather/{rowKey:string}")] HttpRequest req,
            string rowKey,
            ILogger log)
        {
            return await ExecuteAsync(req, log, async () =>
            {
                var weatherData = await _weatherService.GetAsync(rowKey, CancellationToken.None);
                return new OkObjectResult(weatherData);
            });
        }

        [FunctionName("GetWeatherByRange")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/v1/weather/range")] HttpRequest req,
            ILogger log)
        {
            if (!DateTimeOffset.TryParse(req.Query["from"], out var from))
            {
                return new BadRequestObjectResult("Missing or invalid 'from' parameter.");
            }

            if (!DateTimeOffset.TryParse(req.Query["to"], out var to))
            {
                return new BadRequestObjectResult("Missing or invalid 'to' parameter.");
            }

            return await ExecuteAsync(req, log, async () =>
            {
                var weatherData = await _weatherService.GetAsync(from, to, CancellationToken.None);
                return new OkObjectResult(weatherData);
            });
        }
    }
}
