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
    public class MarketRepo : IMarketRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MarketRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMarket(Market market)
        {
            //for saving only properties of the market entity only
            //_context.Entry(market).State = EntityState.Added;
            // for saving related data to db
            _context.Markets.AddAsync(market);
        }

        public void DeleteMarket(Market market)
        {
            _context.Markets.Remove(market);
        }

        public async Task<Market> GetMarket(int id)
        {
            return await _context.Markets
                .Include(x => x.Points)
                .Include(x => x.Barriers)
                .Include(x => x.Photos)
                .Include(X => X.Shop)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MarketDto> GetMarketAsync(string name)
        {
            var market =_context.Markets
                .Include(x => x.Points)
                .Include(x => x.Barriers)
                .Include(x => x.Photos)
                .Include(x => x.Shop)
                .Where(m => m.Name == name)
                .ProjectTo<MarketDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .AsQueryable();

            return await market.SingleOrDefaultAsync();
        }

        public async Task<MarketDto> GetMarketAsync(int id)
        {
            var market =_context.Markets
                .Include(x => x.Points)
                .Include(x => x.Photos)
                .Include(x => x.Shop)
                .Where(m => m.Id == id)
                .ProjectTo<MarketDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .AsQueryable();

            return await market.SingleOrDefaultAsync();
        }

        public async Task<PagedList<MarketDto>> GetMarketsAsync(MarketParams marketParams)
        {
            var query = _context.Markets
                // .Include(m => m.Points)
                // .Include(x => x.Photos)
                // .Include(x => x.Shop)
                .Where(m => m.isDisabled == false && m.isOpen == true)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(marketParams.Search))
                query = query.Where(q => q.Name.Contains(marketParams.Search));
            
            if (!string.IsNullOrWhiteSpace(marketParams.SearchByCity))
                query = query.Where(q => q.City.Contains(marketParams.SearchByCity));

            query = marketParams.OrderBy switch
            {
                "Created" => query.OrderByDescending(m => m.CreatedAt),
                _ => query.OrderBy(m => m.CreatedAt)
            };

            return await PagedList<MarketDto>.CreateAsync(
                query.ProjectTo<MarketDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
                    marketParams.PageNumber, marketParams.PageSize);
        }

        public async Task<bool> MarketExists(string name) 
            => await _context.Markets.AnyAsync(m => m.Name.Contains(name));

        public void RemoveBarrier(Point barrier)
        {
            _context.Points.Remove(barrier);
        }

        public void RemoveWay(Point point)
        {
            _context.Points.Remove(point);
        }

        public void UpdateMarket(Market market)
        {
            _context.Markets.Update(market);
        }
    }
}