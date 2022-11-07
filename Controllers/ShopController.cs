using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Extensions;
using FleaApp_Api.Helpers;
using FleaApp_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public ShopController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("get-shop-by-id/{id}")]
        public async Task<ActionResult<ShopDto>> GetShop(int id)
        {
            var shop = await _uow.ShopRepo.GetShopAsync(id);

            if (shop is not null) return shop;

            return NotFound("There is no findings :)");
        }

        [HttpGet("get-shop-by-name/{name}")]
        public async Task<ActionResult<ShopDto>> GetShop(string name)
        {
            var shop = await _uow.ShopRepo.GetShopAsync(name);

            if (shop is not null) return shop;

            return NotFound("There is no findings :)");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShops([FromQuery] ShopParams shopParams)
        {
            var shops = await _uow.ShopRepo.GetShopsAsync(shopParams);
            Response.AddPaginationHeader(shops.CurrentPage, shops.PageSize, shops.TotalCount, shops.TotalPages);

            if (shops.Count > 0) return Ok(shops);

            return NotFound("There is no findings :)");
        }

        [HttpPost("create-shop")]
        public async Task<ActionResult> CreateShop(CreateShopDto shopDto)
        {
            shopDto.Name = shopDto.Name.ToLower();

            var shop = _mapper.Map<Shop>(shopDto);
            shop.CreatedAt = DateTime.Now;
            shop.isOpen = true;
            shop.isDisabled = false;

            _uow.ShopRepo.AddShop(shop);
            if (await _uow.Complete()) return Ok("Successfully Added");

            return BadRequest("Error Creating entity");
        }

        [HttpPut("update-shop")]
        public async Task<ActionResult> UpdateShop(UpdateShopDto updateShop)
        {
            var shop = await _uow.ShopRepo.GetShop(updateShop.Id);
            _mapper.Map(updateShop, shop);
            shop.Name = shop.Name.ToLower();
            
            _uow.ShopRepo.UpdateShop(shop);

            if (await _uow.Complete()) return Ok("Successfully updated");

            return BadRequest("Error Updating entity");
        }

        [HttpDelete("delete-shop/{id}")]
        public async Task<ActionResult> DeleteShop(int id)
        {
            var shop = await _uow.ShopRepo.GetShop(id);
            _uow.ShopRepo.DeleteShop(shop);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }
    }
}