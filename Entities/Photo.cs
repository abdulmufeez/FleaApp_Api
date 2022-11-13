using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleaApp_Api.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        [Required] public string Url { get; set; }        
        public bool IsMain { get; set; } = false;       
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public AppUser User { get; set; }        
        public Market Market { get; set; }        
        public Shop Shop { get; set; }        
        public Product Product { get; set; }        
    }
}