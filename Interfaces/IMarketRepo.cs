using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Helpers;

namespace FleaApp_Api.Interfaces
{
    public interface IMarketRepo
    {
        void AddMarket (Market market);
        void UpdateMarket (Market market);
        void DeleteMarket (Market market);
        Task<bool> MarketExists(string name);
        Task<MarketDto> GetMarketAsync (string name);
        Task<MarketDto> GetMarketAsync (int id);     
        Task<Market> GetMarket(int id); 
        
        Task<PagedList<MarketDto>> GetMarketsAsync(MarketParams marketParams);      
    }
}