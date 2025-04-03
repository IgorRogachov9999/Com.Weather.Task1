using Com.Weather.Task1.Domain.Options;
using Microsoft.Extensions.DependencyInjection;
using Com.Weather.Task1.Domain.Extensions;

namespace Com.Weather.Task1.WeatherFunction.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddOptions<StorageAccountOptions>(StorageAccountOptions.SectionName);
            services.AddOptions<WeatherStorageOptions>(WeatherStorageOptions.SectionName);

            services.AddDomainServices();

            return services;
        }
    }
}
