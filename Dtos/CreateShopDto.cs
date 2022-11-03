namespace FleaApp_Api.Dtos
{
    public class CreateShopDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }        
        public string Location { get; set; }
        public int MarketId { get; set; }

        public List<GeoLocationDto> Points { get; set; }
    }
}