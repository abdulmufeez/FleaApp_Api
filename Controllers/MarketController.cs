using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Extensions;
using FleaApp_Api.Helpers;
using FleaApp_Api.Interfaces;
using FleaApp_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class MarketController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;        
        private readonly IPhotoService _photoService;
        public MarketController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _uow = uow;
        }
        
        [Authorize]
        [HttpGet("get-market-by-id/{id}", Name = "GetMarket")]
        public async Task<ActionResult<MarketDto>> GetMarket(int id)
        {
            var market = await _uow.MarketRepo.GetMarketAsync(id);

            if (market is not null) return market;

            return NotFound("There is no findings :)");
        }

        [Authorize]
        [HttpGet("get-market-by-name/{name}")]
        public async Task<ActionResult<MarketDto>> GetMarket(string name)
        {
            var market = await _uow.MarketRepo.GetMarketAsync(name);

            if (market is not null) return market;

            return NotFound("There is no findings :)");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarketDto>>> GetMarkets ([FromQuery] MarketParams marketParams)
        {   
            var markets = await _uow.MarketRepo.GetMarketsAsync(marketParams);
            Response.AddPaginationHeader(markets.CurrentPage, markets.PageSize, markets.TotalCount,
                markets.TotalPages);
            
            if (markets.Count > 0) return Ok(markets);
            
            return NotFound("There is no findings :)");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-market")]
        public async Task<ActionResult> CreateMarket(CreateMarketDto marketDto)
        {
            marketDto.Name = marketDto.Name.ToLower(); 
            marketDto.City = marketDto.City.ToLower();
            marketDto.Country = marketDto.Country.ToLower();            

            if (!await _uow.MarketRepo.MarketExists(marketDto.Name))
            {
                var market = _mapper.Map<Market>(marketDto);
                market.CreatedAt = DateTime.Now;
                market.isOpen = true;
                market.isDisabled = false;

                _uow.MarketRepo.AddMarket(market);
                if (await _uow.Complete()) 
                    return Ok("Successfully Added");                                //RedirectToAction("AddPhoto", new {market.Id, marketDto.Photo});
            }
            else return BadRequest("Name already exists");   

            return BadRequest("Error Creating entity");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("update-market")]
        public async Task<ActionResult> UpdateMarket(UpdateMarketDto updateMarket)
        {
            var market = await _uow.MarketRepo.GetMarket(updateMarket.Id);
            _mapper.Map(updateMarket, market);
            market.Name = market.Name.ToLower();
            market.City = market.City.ToLower();
            market.Country = market.Country.ToLower();
            _uow.MarketRepo.UpdateMarket(market);

            if (await _uow.Complete()) return Ok("Successfully updated");

            return BadRequest("Error Updating entity");
        }

        [Authorize(Policy = "RequireAdminRole")]        
        [HttpDelete("delete-market/{id}")]
        public async Task<ActionResult> DeleteMarket(int id)
        {
            var market = await _uow.MarketRepo.GetMarket(id);
            _uow.MarketRepo.DeleteMarket(market);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }

        //photo
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm]CreatePhotoDto photoDto)
        { 
            var market = await _uow.MarketRepo.GetMarket(photoDto.Id);
            var result = await _photoService.AddPhotoAsync(photoDto.Photo);
            
            if (result.Error is not null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };            

            market.Photos.Add(photo);

            if (await _uow.Complete())
            {
                // this will include the location in headers where you get the photos
                return CreatedAtRoute("GetMarket", new { id = market.Id }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Adding Photo");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(SetPhotoMainDto photoMainDto)
        {
            var market = await _uow.MarketRepo.GetMarket(photoMainDto.Id);

            var photo = market.Photos.SingleOrDefault(p => p.Id == photoMainDto.PhotoId);

            if (photo.IsMain) return BadRequest("This Photo is already a main photo");

            var currentMainPhoto = market.Photos.SingleOrDefault(p => p.IsMain);

            if (currentMainPhoto != null) currentMainPhoto.IsMain = false;

            photo.IsMain = true;            

            if (await _uow.Complete()) return Ok($"Set {photo.Url} as main profile pic");            

            return BadRequest("Failed to set main photo");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto(DeletePhotoDto photoDto)
        {
            var market = await _uow.MarketRepo.GetMarket(photoDto.Id);
            var photo = market.Photos.SingleOrDefault(p => p.Id == photoDto.PhotoId);

            if (photo == null) return NotFound("There is no findings :)");
            if (photo.IsMain) return BadRequest("Cannot delete main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            market.Photos.Remove(photo);
            
            if (await _uow.Complete()) return Ok("Successfully deleted photo");            

            return BadRequest("Error deleting the photo");
        }
    }
}