using AutoMapper;
using AutoMapper.QueryableExtensions;
using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class GeoCordinatesRepo : IGeoCordinatesRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public GeoCordinatesRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<PointDto>> GetMarketCordinate(int id)
        {
            return await _context.GeoLocations
                .Where(x => x.MarketId == id)
                .OrderBy(x => x.Id)
                .ProjectTo<PointDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<PointDto>> GetShopCordinate(int id)
        {
            return await _context.GeoLocations
                .Where(x => x.ShopId == id)
                .OrderBy(x => x.Id)
                .ProjectTo<PointDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}