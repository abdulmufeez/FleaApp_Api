using System.ComponentModel.DataAnnotations.Schema;
using FleaApp_Api.Extensions;

namespace FleaApp_Api.Entities
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool isSoldOut { get; set; } = false;
        public DateTime CreatedAt { get; set; } = (DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)).SetKindUtc();


        public Shop Shop { get; set; }
        public int ShopId { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public SubCategory SubCategory { get; set; }
        public int SubCategoryId { get; set; }
    }
}