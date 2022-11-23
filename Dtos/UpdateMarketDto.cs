namespace FleaApp_Api.Dtos
{
    public class UpdateMarketDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public GeoLocationDto CenterPoint { get; set; }     


        public ICollection<GeoLocationDto> Points { get; set; }
    }
}