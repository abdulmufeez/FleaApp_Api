using FleaApp_Api.Dtos;
using FleaApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class GeoCordinatesController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public GeoCordinatesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize]
        [HttpGet("get-location-of-market/{id}")]
        public async Task<ActionResult<IEnumerable<PointDto>>> GetMarketPoint(int id)
        {
            var cordinates = await _uow.GeoCordinatesRepo.GetMarketCordinate(id);

            if (cordinates.Count() > 0) return Ok(cordinates);

            return BadRequest("There is no findings :)");
        }

        [Authorize]
        [HttpGet("get-location-of-shop/{id}")]
        public async Task<ActionResult<IEnumerable<PointDto>>> GetShopPoint(int id)
        {
            var cordinates = await _uow.GeoCordinatesRepo.GetShopCordinate(id);

            if (cordinates.Count() > 0) return Ok(cordinates);

            return BadRequest("There is no findings :)");
        }
    }
}