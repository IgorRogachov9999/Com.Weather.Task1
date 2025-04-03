using Com.Weather.Task1.WeatherFunction.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Com.Weather.Task1.WeatherFunction.Startup))]

namespace Com.Weather.Task1.WeatherFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddServices();
        }
    }
}
