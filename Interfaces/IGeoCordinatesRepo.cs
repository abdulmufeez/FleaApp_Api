using FleaApp_Api.Dtos;

namespace FleaApp_Api.Interfaces
{
    public interface IGeoCordinatesRepo
    {
        Task<IEnumerable<GeoLocationDto>> GetMarketCordinate(int id);
        Task<IEnumerable<GeoLocationDto>> GetShopCordinate(int id);
    }
}