namespace FleaApp_Api.Dtos
{
    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }        
        public string Location { get; set; }        
        public DateTime CreatedAt { get; set; }
        public string MainPhotoUrl { get; set; }
        public int MarketId { get; set; }  

        public ICollection<PointDto> Points { get; set; }        
        public ICollection<PhotoDto> Photos { get; set; }                      
        public ICollection<ProductDto> Products { get; set; }
    }
}