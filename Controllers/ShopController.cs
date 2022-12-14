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
    public class ShopController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;
        public ShopController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }

        [Authorize]
        [HttpGet("get-shop-by-id/{id}", Name = "GetShop")]
        public async Task<ActionResult<ShopDto>> GetShop(int id)
        {
            var shop = await _uow.ShopRepo.GetShopAsync(id);

            if (shop is not null) return shop;

            return NotFound("There is no findings :)");
        }

        [Authorize]
        [HttpGet("get-shop-by-name/{name}")]
        public async Task<ActionResult<ShopDto>> GetShop(string name)
        {
            var shop = await _uow.ShopRepo.GetShopAsync(name);

            if (shop is not null) return shop;

            return NotFound("There is no findings :)");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShops([FromQuery] ShopParams shopParams)
        {
            var shops = await _uow.ShopRepo.GetShopsAsync(shopParams);
            Response.AddPaginationHeader(shops.CurrentPage, shops.PageSize, shops.TotalCount, shops.TotalPages);

            if (shops.Count > 0) return Ok(shops);

            return NotFound("There is no findings :)");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPost("create-shop")]
        public async Task<ActionResult> CreateShop(CreateShopDto shopDto)
        {
            if (shopDto.MarketId > 0)
            {
                shopDto.Name = shopDto.Name.ToLower();

                var shop = _mapper.Map<Shop>(shopDto);
                shop.CreatedAt = (DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)).SetKindUtc();
                shop.isOpen = true;
                shop.isDisabled = false;
                shop.AppUserId = User.GetAppUserId();

                //shop.Points.First().Status = StatusEnum.EntryPoint;
                foreach (var item in shop.Points)
                {
                    item.Status = StatusEnum.Boundry;
                    item.MarketId = shop.MarketId;
                }
                shop.Points.Add(
                    new Point
                    {
                        Latitude = shopDto.CenterPoint.Latitude,
                        Longitude = shopDto.CenterPoint.Longitude,
                        Status = StatusEnum.CenterPoint,
                        MarketId = shop.MarketId
                    });

                if (await _uow.ShopRepo.AddShop(shop))
                {
                    if (await _uow.Complete()) return Ok("Successfully Added");

                    return BadRequest("Error Creating entity");
                }

                return BadRequest("You must step on another one's property when creating your property!");
            }
            else return BadRequest("You must have to provide market Id");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPut("update-shop")]
        public async Task<ActionResult> UpdateShop(UpdateShopDto updateShop)
        {
            var shop = await _uow.ShopRepo.GetShop(updateShop.Id);
            _mapper.Map(updateShop, shop);
            shop.Name = shop.Name.ToLower();
            shop.AppUserId = User.GetAppUserId();

            if (updateShop.Points.Count > 0)
            {
                //shop.Points.First().Status = StatusEnum.EntryPoint;
                foreach (var item in shop.Points)
                {
                    item.Status = StatusEnum.Boundry;
                }
                if (updateShop.CenterPoint is not null)
                {
                    shop.Points.Add(
                    new Point
                    {
                        Latitude = updateShop.CenterPoint.Latitude,
                        Longitude = updateShop.CenterPoint.Longitude,
                        Status = StatusEnum.CenterPoint
                    });
                }
            }

            if (await _uow.ShopRepo.UpdateShop(shop))
            {
                if (await _uow.Complete()) return Ok("Successfully Added");

                return BadRequest("Error Creating entity");
            }

            return BadRequest("You must step on another one's property when creating your property!");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpDelete("delete-shop/{id}")]
        public async Task<ActionResult> DeleteShop(int id)
        {
            var shop = await _uow.ShopRepo.GetShop(id);
            _uow.ShopRepo.DeleteShop(shop);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }

        //photo
        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm] CreatePhotoDto photoDto)
        {
            var shop = await _uow.ShopRepo.GetShop(photoDto.Id);
            var result = await _photoService.AddPhotoAsync(photoDto.Photo);

            if (result.Error is not null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            shop.Photos.Add(photo);

            if (await _uow.Complete())
            {
                // this will include the location in headers where you get the photos
                return CreatedAtRoute("GetShop", new { id = shop.Id }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Adding Photo");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(SetPhotoMainDto photoMainDto)
        {
            var shop = await _uow.ShopRepo.GetShop(photoMainDto.Id);

            var photo = shop.Photos.SingleOrDefault(p => p.Id == photoMainDto.PhotoId);

            if (photo.IsMain) return BadRequest("This Photo is already a main photo");

            var currentMainPhoto = shop.Photos.SingleOrDefault(p => p.IsMain);

            if (currentMainPhoto != null) currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            if (await _uow.Complete()) return Ok($"Set {photo.Url} as main profile pic");

            return BadRequest("Failed to set main photo");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto(DeletePhotoDto photoDto)
        {
            var shop = await _uow.ShopRepo.GetShop(photoDto.Id);
            var photo = shop.Photos.SingleOrDefault(p => p.Id == photoDto.PhotoId);

            if (photo == null) return NotFound("There is no findings :)");
            if (photo.IsMain) return BadRequest("Cannot delete main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            shop.Photos.Remove(photo);

            if (await _uow.Complete()) return Ok("Successfully deleted photo");

            return BadRequest("Error deleting the photo");
        }
    }
}