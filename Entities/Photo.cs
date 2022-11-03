using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleaApp_Api.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        [Required] public string Url { get; set; }        
        public bool IsMain { get; set; }
        public bool IsApprove { get; set; } = true;
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Market Market { get; set; }
        public int? MarketId { get; set; }
        public Shop Shop { get; set; }
        public int? ShopId { get; set; }
        public Product Product { get; set; }
        public int? ProductId { get; set; }
    }
}