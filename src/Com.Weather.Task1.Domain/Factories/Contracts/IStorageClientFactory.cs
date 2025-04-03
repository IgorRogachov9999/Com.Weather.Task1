using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace Com.Weather.Task1.Domain.Factories.Contracts
{
    public interface IStorageClientFactory
    {
        BlobServiceClient GetBlobServiceClient();

        TableServiceClient GetTableServiceClient();
    }
}
