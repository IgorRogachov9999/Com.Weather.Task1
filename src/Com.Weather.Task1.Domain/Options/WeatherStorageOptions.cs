namespace Com.Weather.Task1.Domain.Options
{
    public class WeatherStorageOptions
    {
        public const string SectionName = "WeatherStorage";

        public string BlobName { get; set; }

        public string TableName { get; set; }
    }
}
