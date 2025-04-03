using Com.Weather.Task1.Domain.Extensions;
using Com.Weather.Task1.Domain.Options;
using Com.Weather.Task1.Domain.Services;
using Com.Weather.Task1.Domain.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Weather.Task1.WeatherPollerFunction.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddOptions<OpenWeatherMapOptions>(OpenWeatherMapOptions.SectionName);
            services.AddOptions<StorageAccountOptions>(StorageAccountOptions.SectionName);
            services.AddOptions<WeatherStorageOptions>(WeatherStorageOptions.SectionName);

            services.AddDomainServices();
            services.AddScoped<IOpenWeatherMapClientService, OpenWeatherMapClientService>();
            services.AddScoped<IWeatherPollerService, WeatherPollerService>();

            return services;
        }
    }
}
