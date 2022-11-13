using System.ComponentModel.DataAnnotations;

namespace FleaApp_Api.Dtos
{
    public class UserRegisterDto
    {
        [Required] public string Username { get; set; }        
        [Required] public string Email { get; set; }        
        [Required] public string Password { get; set; }  
        public IFormFile Photo { get; set; } 

        public bool isUser { get; set; } = true;
    }
}