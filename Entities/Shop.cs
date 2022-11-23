using System.ComponentModel.DataAnnotations.Schema;

namespace FleaApp_Api.Entities
{
    [Table("Shops")]
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }        
        public string Location { get; set; }
        public bool isOpen { get; set; }
        public bool isDisabled { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        


        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public ICollection<Point> Points { get; set; }
        public Market Market { get; set; }
        public int MarketId { get; set; }
        public ICollection<Product> Product { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}