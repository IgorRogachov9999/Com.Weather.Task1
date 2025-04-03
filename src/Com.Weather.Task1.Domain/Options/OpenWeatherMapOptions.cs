namespace Com.Weather.Task1.Domain.Options
{
    public class OpenWeatherMapOptions
    {
        public const string SectionName = "OpenWeatherMap";

        public string ApiKey { get; set; }

        public string BaseUrl { get; set; }
    }
}
