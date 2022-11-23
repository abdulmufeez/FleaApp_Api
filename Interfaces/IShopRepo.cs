using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Helpers;

namespace FleaApp_Api.Interfaces
{
    public interface IShopRepo
    {
        Task<bool> AddShop (Shop shop);
        Task<bool> UpdateShop (Shop shop);
        void DeleteShop (Shop shop);        
        Task<ShopDto> GetShopAsync (string name);
        Task<ShopDto> GetShopAsync (int id);     
        Task<Shop> GetShop(int id); 
        
        Task<PagedList<ShopDto>> GetShopsAsync(ShopParams shopParams);  
    }
}