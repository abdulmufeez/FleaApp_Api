using System.ComponentModel.DataAnnotations.Schema;
using FleaApp_Api.Extensions;

namespace FleaApp_Api.Entities
{
    [Table("SubCategories")]
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = (DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)).SetKindUtc();

        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}