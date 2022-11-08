using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Extensions;
using FleaApp_Api.Helpers;
using FleaApp_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _uow.ProductRepo.GetProductAsync(id);

            if (product is not null) return product;

            return NotFound("There is no findings :)");
        }

        [HttpGet("get-product-by-name/{name}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string name)
        {
            var product = await _uow.ProductRepo.GetProductAsync(name);

            if (product is not null) return product;

            return NotFound("There is no findings :)");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GgetProducts([FromQuery] ProductParams productParams)
        {
            var products = await _uow.ProductRepo.GetProductsAsync(productParams);
            Response.AddPaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

            if (products.Count > 0) return Ok(products);

            return NotFound("There is no findings :)");
        }  

        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct(CreateProductDto productDto)
        {
            productDto.Name = productDto.Name.ToLower();

            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.Now;
            product.isSoldOut = false;

            _uow.ProductRepo.AddProduct(product);
            if (await _uow.Complete()) return Ok("Successfully Added");

            return BadRequest("Error Creating entity");
        }

        [HttpPut("update-product")]
        public async Task<ActionResult> UpdateProduct(UpdateProductDto productDto)
        {
            var product = await _uow.ProductRepo.GetProduct(productDto.Id);
            _mapper.Map(productDto, product);
            product.Name = product.Name.ToLower();
            
            _uow.ProductRepo.UpdateProduct(product);

            if (await _uow.Complete()) return Ok("Successfully updated");

            return BadRequest("Error Updating entity");
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _uow.ProductRepo.GetProduct(id);
            _uow.ProductRepo.DeleteProduct(product);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }
    }
}