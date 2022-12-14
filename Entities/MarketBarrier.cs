namespace FleaApp_Api.Entities
{
    public class MarketBarrier
    {
        public int Id { get; set; }
        public ICollection<Point> BarrierPoints { get; set; } = new List<Point>();

        public Market Market { get; set; }
        public int MarketId { get; set; }
    }
}