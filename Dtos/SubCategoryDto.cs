namespace FleaApp_Api.Dtos
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int CategoryId { get; set; }
    }
}