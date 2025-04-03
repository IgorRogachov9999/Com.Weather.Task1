using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Com.Weather.Task1.Domain.Factories.Contracts;
using Com.Weather.Task1.Domain.Options;
using Microsoft.Extensions.Options;

namespace Com.Weather.Task1.Domain.Factories
{
    public class StorageClientFactory : IStorageClientFactory
    {
        private readonly StorageAccountOptions _storageAccountOptions;

        public StorageClientFactory(IOptionsMonitor<StorageAccountOptions> options)
        {
            _storageAccountOptions = options.CurrentValue;
        }

        public BlobServiceClient GetBlobServiceClient()
        {
            return new BlobServiceClient(_storageAccountOptions.ConnectionString);
        }

        public TableServiceClient GetTableServiceClient()
        {
            return new TableServiceClient(_storageAccountOptions.ConnectionString);
        }
    }
}
