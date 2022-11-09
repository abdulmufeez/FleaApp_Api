namespace FleaApp_Api.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool isSoldOut { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public string MainPhotoUrl { get; set; }

        
        public int ShopId { get; set; }
        public int SubCategoryId { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}