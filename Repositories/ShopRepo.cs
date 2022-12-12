using AutoMapper;
using AutoMapper.QueryableExtensions;
using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Helpers;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class ShopRepo : IShopRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ShopRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> AddShop(Shop shop)
        {
            foreach (var item in shop.Points)
            {
                var point = await _context.Points
                    .Where(x => x.Latitude == item.Latitude && 
                        x.Longitude == item.Longitude).SingleOrDefaultAsync();
                
                if(point is not null)
                {                    
                    return false;                                        
                }
            }

            await _context.Shops.AddAsync(shop);
            return true;
        }

        public void DeleteShop(Shop shop)
        {
            _context.Shops.Remove(shop);
        }

        public async Task<Shop> GetShop(int id)
        {
            return await _context.Shops
                .Include(s => s.Points)
                .Include(s => s.Market)
                .Include(s => s.Photos)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ShopDto> GetShopAsync(string name)
        {
            var shop =_context.Shops
                .Include(x => x.Points)
                .Include(x => x.Photos)
                .Include(x => x.Market.Points)
                .Where(m => m.Name == name)
                .ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .AsQueryable();

            return await shop.SingleOrDefaultAsync();
        }

        public async Task<ShopDto> GetShopAsync(int id)
        {
             var shop =_context.Shops
                .Include(x => x.Points)
                .Include(x => x.Photos)
                .Include(x => x.Market.Points)
                .Where(m => m.Id == id)
                .ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .AsQueryable();

            return await shop.SingleOrDefaultAsync();
        }

        public async Task<PagedList<ShopDto>> GetShopsAsync(ShopParams shopParams)
        {
             var query = _context.Shops
                .Include(m => m.Points)
                .Include(x => x.Photos)
                .Where(m => m.isDisabled == false && m.isOpen == true)
                .AsQueryable();

            if (shopParams.SearchByMarketId > 0)
                query = query.Where(q => q.MarketId == shopParams.SearchByMarketId);

            if (!string.IsNullOrWhiteSpace(shopParams.Search))
                query = query.Where(q => q.Name.Contains(shopParams.Search));                        

            query = shopParams.OrderBy switch
            {
                "Created" => query.OrderByDescending(m => m.CreatedAt),
                _ => query.OrderBy(m => m.CreatedAt)
            };

            return await PagedList<ShopDto>.CreateAsync(
                query.ProjectTo<ShopDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
                    shopParams.PageNumber, shopParams.PageSize);
        }

        public async Task<bool> UpdateShop(Shop shop)
        {
            // foreach (var item in shop.Points)
            // {
            //     var point = await _context.Points
            //         .Where(x => x.Latitude == item.Latitude && 
            //             x.Longitude == item.Longitude && 
            //             x.MarketId == item.MarketId &&
            //             x.Status == StatusEnum.Way).SingleOrDefaultAsync();
                
            //     if(point is not null)
            //     {
            //         point.ShopId = item.ShopId;
            //         point.Status = StatusEnum.Boundry;

            //         shop.Points.Remove(item);
            //     }
            // }
            foreach (var item in shop.Points)
            {
                var point = await _context.Points
                    .Where(x => x.Latitude == item.Latitude && 
                        x.Longitude == item.Longitude).SingleOrDefaultAsync();
                
                if(point is not null)
                {                    
                    return false;                                        
                }
            }

            _context.Shops.Update(shop);
            return true;
        }
    }
}