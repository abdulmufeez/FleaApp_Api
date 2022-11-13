using Microsoft.AspNetCore.Identity;

namespace FleaApp_Api.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; }
        public Photo ProfilePhoto { get; set; }
        public int? ProfilePhotoId { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}