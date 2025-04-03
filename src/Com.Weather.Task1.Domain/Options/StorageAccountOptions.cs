namespace Com.Weather.Task1.Domain.Options
{
    public class StorageAccountOptions
    {
        public const string SectionName = "StorageAccount";

        public bool UseManagedIdentity { get; set; } = false;

        public string StorageAccountName { get; set; }

        public string ConnectionString { get; set; }
    }
}
