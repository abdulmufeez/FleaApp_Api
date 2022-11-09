namespace FleaApp_Api.Entities
{
    public class GeoLocation
    {
        public int Id { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }       

        public Market Market { get; set; } 
        public int? MarketId { get; set; } 
        public Shop Shop { get; set; }        
        public int? ShopId { get; set; }
    }
}