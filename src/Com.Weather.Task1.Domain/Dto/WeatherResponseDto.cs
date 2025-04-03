namespace Com.Weather.Task1.Domain.Dto
{
    public class WeatherResponseDto
    {
        public CoordDto Coord { get; set; }
        public IEnumerable<WeatherDto> Weather { get; set; }
        public string Base { get; set; }
        public MainDto Main { get; set; }
        public int Visibility { get; set; }
        public WindDto Wind { get; set; }
        public CloudsDto Clouds { get; set; }
        public long Dt { get; set; }
        public SysDto Sys { get; set; }
        public int Timezone { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }
    }
}
