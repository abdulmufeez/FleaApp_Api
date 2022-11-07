namespace FleaApp_Api.Dtos
{
    public class UpdateSubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int CategoryId { get; set; }
    }
}