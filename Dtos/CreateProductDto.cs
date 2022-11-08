namespace FleaApp_Api.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }        

        
        public int ShopId { get; set; }
        //public ICollection<Photo> Photos { get; set; }
        public int SubCategoryId { get; set; }
 
    }
}