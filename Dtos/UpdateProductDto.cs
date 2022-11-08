namespace FleaApp_Api.Dtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool isSoldOut { get; set; } 

        
        public int ShopId { get; set; }
        //public ICollection<Photo> Photos { get; set; }
        public int SubCategoryId { get; set; }
    }
}