using Microsoft.AspNetCore.Identity;

namespace FleaApp_Api.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    }
}