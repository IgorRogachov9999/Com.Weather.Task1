namespace Com.Weather.Task1.Domain.Dto
{
    public class MainDto
    {
        public decimal Temp { get; set; }
        public decimal Feels_like { get; set; }
        public decimal Temp_min { get; set; }
        public decimal Temp_max { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int Sea_level { get; set; }
        public int Grnd_level { get; set; }
    }
}
