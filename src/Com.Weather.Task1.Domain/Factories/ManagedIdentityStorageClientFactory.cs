using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Blobs;
using Com.Weather.Task1.Domain.Factories.Contracts;
using Com.Weather.Task1.Domain.Options;
using Microsoft.Extensions.Options;

namespace Com.Weather.Task1.Domain.Factories
{
    public class ManagedIdentityStorageClientFactory : IStorageClientFactory
    {
        private readonly DefaultAzureCredential _credential;
        private readonly string _accountName;

        public ManagedIdentityStorageClientFactory(DefaultAzureCredential credential, IOptionsMonitor<StorageAccountOptions> options)
        {
            _credential = credential;
            _accountName = options.CurrentValue.StorageAccountName;
        }

        public BlobServiceClient GetBlobServiceClient()
        {
            var uri = new Uri($"https://{_accountName}.blob.core.windows.net");
            return new BlobServiceClient(uri, _credential);
        }

        public TableServiceClient GetTableServiceClient()
        {
            var uri = new Uri($"https://{_accountName}.table.core.windows.net");
            return new TableServiceClient(uri, _credential);
        }
    }
}
