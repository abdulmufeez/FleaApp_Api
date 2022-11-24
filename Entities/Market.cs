using System.ComponentModel.DataAnnotations.Schema;

namespace FleaApp_Api.Entities
{
    [Table("Markets")]
    public class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public bool isOpen { get; set; }
        public bool isDisabled { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        

        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public ICollection<Point> Points { get; set; } = new List<Point>();
        public ICollection<MarketBarrier> Barriers { get; set; } = new List<MarketBarrier>();
        public ICollection<Shop> Shop { get; set; } = new List<Shop>();
        public ICollection<Photo> Photos { get; set; } =  new List<Photo>();
    }
}