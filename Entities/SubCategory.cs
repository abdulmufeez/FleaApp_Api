using System.ComponentModel.DataAnnotations.Schema;

namespace FleaApp_Api.Entities
{
    [Table("SubCategories")]
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}