namespace FleaApp_Api.Dtos
{
    public class MakeWayDto
    {
        public int MarketId { get; set; }
        public ICollection<PointDto> WayPoints { get; set; }
    }
}