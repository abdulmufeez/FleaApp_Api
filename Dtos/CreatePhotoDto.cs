namespace FleaApp_Api.Dtos
{
    public class CreatePhotoDto
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
    }
}