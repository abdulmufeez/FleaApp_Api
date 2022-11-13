using System.Security.Claims;

namespace FleaApp_Api.Extensions
{
    public static class ClaimPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }       
        public static int GetAppUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }       
    }
}