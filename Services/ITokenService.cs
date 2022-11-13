using FleaApp_Api.Entities;

namespace FleaApp_Api.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync (AppUser user);
    }
}