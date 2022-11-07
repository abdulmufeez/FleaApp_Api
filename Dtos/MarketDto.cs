namespace FleaApp_Api.Dtos
{
    public class MarketDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }


        public List<GeoLocationDto> Points { get; set; }
    }
}