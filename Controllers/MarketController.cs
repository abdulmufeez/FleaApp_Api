using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Extensions;
using FleaApp_Api.Helpers;
using FleaApp_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class MarketController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;        
        public MarketController(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [HttpGet("get-market-by-id/{id}")]
        public async Task<ActionResult<MarketDto>> GetMarket(int id)
        {
            var market = await _uow.MarketRepo.GetMarketAsync(id);

            if (market is not null) return market;

            return BadRequest("There is no findings");
        }

        [HttpGet("get-market-by-name/{name}")]
        public async Task<ActionResult<MarketDto>> GetMarket(string name)
        {
            var market = await _uow.MarketRepo.GetMarketAsync(name);

            if (market is not null) return market;

            return BadRequest("There is no findings");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarketDto>>> GetMarkets ([FromQuery] MarketParams marketParams)
        {   
            var markets = await _uow.MarketRepo.GetMarketsAsync(marketParams);
            Response.AddPaginationHeader(markets.CurrentPage, markets.PageSize, markets.TotalCount,
                markets.TotalPages);
            
            if (markets.Count > 0) return Ok(markets);
            
            return NotFound("There is no findings :)");
        }

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
                if (await _uow.Complete()) return Ok("Successfully Added");                                
            }
            else return BadRequest("Name already exists");   

            return BadRequest("Error Creating entity");
        }

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

        [HttpDelete("delete-market/{id}")]
        public async Task<ActionResult> DeleteMarket(int id)
        {
            var market = await _uow.MarketRepo.GetMarket(id);
            _uow.MarketRepo.DeleteMarket(market);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }
    }
}