namespace FleaApp_Api.Dtos
{
    public class LocationPathDto
    {
        public int MarketId { get; set; }
        public PointDto CurrentPoint { get; set; }
        public PointDto DesiredPoint { get; set; }
    }
}