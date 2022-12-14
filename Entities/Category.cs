using System.ComponentModel.DataAnnotations.Schema;
using FleaApp_Api.Extensions;

namespace FleaApp_Api.Entities
{
    [Table("Categories")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = (DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)).SetKindUtc();


        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public ICollection<SubCategory> SubCategory { get; set; } = new List<SubCategory>();
    }
}