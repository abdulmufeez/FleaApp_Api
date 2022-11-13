using FleaApp_Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Data.SeedData
{
    public class SeedData
    {
        public static async Task SeedAppUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // check if already seed
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Admin"},
                new AppRole{Name = "ShopKeeper"},
                new AppRole{Name = "User"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                CreatedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(admin, "admin@123");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "ShopKeeper" });

            var shopKeepr = new AppUser
            {
                UserName = "shopkeeper",
                Email = "shopkeeper@example.com",
                CreatedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(shopKeepr, "shopkeeper@123");
            await userManager.AddToRoleAsync(shopKeepr, "ShopKeeper");

            var testUser = new AppUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                CreatedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(testUser, "testuser@123");
            await userManager.AddToRoleAsync(testUser, "User");
        }
    }
}