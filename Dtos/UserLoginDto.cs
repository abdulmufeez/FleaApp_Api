using System.ComponentModel.DataAnnotations;

namespace FleaApp_Api.Dtos
{
    public class UserLoginDto
    {
        [Required] public string Username { get; set; }        
        [Required] public string Password { get; set; }        
    }
}