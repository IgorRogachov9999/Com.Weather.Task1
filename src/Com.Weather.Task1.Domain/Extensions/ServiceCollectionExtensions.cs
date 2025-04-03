using Azure.Core;
using Azure.Identity;
using Com.Weather.Task1.Domain.Factories;
using Com.Weather.Task1.Domain.Factories.Contracts;
using Com.Weather.Task1.Domain.Options;
using Com.Weather.Task1.Domain.Services;
using Com.Weather.Task1.Domain.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Weather.Task1.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, WeatherStorageAccountService>();
            services.AddScoped<ITableService, AzureTableService>();
            services.AddScoped<IWeatherService, WeatherService>();

            using var serviceProvider = services.BuildServiceProvider();
            var storageAccountOptions = serviceProvider.GetRequiredService<StorageAccountOptions>();

            if (storageAccountOptions.UseManagedIdentity)
            {
                services.AddSingleton<DefaultAzureCredential, DefaultAzureCredential>();
                services.AddSingleton<IStorageClientFactory, ManagedIdentityStorageClientFactory>();
            }
            else
            {
                services.AddSingleton<IStorageClientFactory, StorageClientFactory>();
            }

            return services;
        }
    }
}
