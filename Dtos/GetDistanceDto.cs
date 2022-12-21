namespace FleaApp_Api.Dtos
{
    public class GetDistanceDto
    {
        public PointDto CurrentPosition { get; set; }
        public int MarketId { get; set; }        
        public int ShopId { get; set; }        
    }
}