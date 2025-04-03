using Com.Weather.Task1.WeatherPollerFunction.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Com.Weather.Task1.WeatherPollerFunction.Startup))]

namespace Com.Weather.Task1.WeatherPollerFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddServices();
        }
    }
}
