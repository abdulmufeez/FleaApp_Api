using FleaApp_Api.Dtos;

namespace FleaApp_Api.Interfaces
{
    public interface IGeoCordinatesRepo
    {
        Task<IEnumerable<PointDto>> GetMarketCordinate(int id);
        Task<IEnumerable<PointDto>> GetShopCordinate(int id);
        Task<IEnumerable<PointDto>> GetLocationPath(LocationPathDto locationPathDto);
    }
}