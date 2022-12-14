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
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;
        public ProductController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }

        [Authorize]
        [HttpGet("get-product-by-id/{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _uow.ProductRepo.GetProductAsync(id);

            if (product is not null) return product;

            return NotFound("There is no findings :)");
        }

        [Authorize]
        [HttpGet("get-product-by-name/{name}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string name)
        {
            var product = await _uow.ProductRepo.GetProductAsync(name);

            if (product is not null) return product;

            return NotFound("There is no findings :)");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GgetProducts([FromQuery] ProductParams productParams)
        {
            var products = await _uow.ProductRepo.GetProductsAsync(productParams);
            Response.AddPaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

            if (products.Count > 0) return Ok(products);

            return NotFound("There is no findings :)");
        }  

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct(CreateProductDto productDto)
        {
            productDto.Name = productDto.Name.ToLower();

            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = (DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)).SetKindUtc();
            product.isSoldOut = false;

            _uow.ProductRepo.AddProduct(product);
            if (await _uow.Complete()) return Ok("Successfully Added");

            return BadRequest("Error Creating entity");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
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

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpDelete("delete-product/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _uow.ProductRepo.GetProduct(id);
            _uow.ProductRepo.DeleteProduct(product);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }

        //photo
        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm]CreatePhotoDto photoDto)
        { 
            var product = await _uow.ProductRepo.GetProduct(photoDto.Id);
            var result = await _photoService.AddPhotoAsync(photoDto.Photo);
            
            if (result.Error is not null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };            

            product.Photos.Add(photo);

            if (await _uow.Complete())
            {
                // this will include the location in headers where you get the photos
                return CreatedAtRoute("GetProduct", new { id = product.Id }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Adding Photo");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(SetPhotoMainDto photoMainDto)
        {
            var product = await _uow.ProductRepo.GetProduct(photoMainDto.Id);

            var photo = product.Photos.SingleOrDefault(p => p.Id == photoMainDto.PhotoId);

            if (photo.IsMain) return BadRequest("This Photo is already a main photo");

            var currentMainPhoto = product.Photos.SingleOrDefault(p => p.IsMain);

            if (currentMainPhoto != null) currentMainPhoto.IsMain = false;

            photo.IsMain = true;            

            if (await _uow.Complete()) return Ok($"Set {photo.Url} as main profile pic");            

            return BadRequest("Failed to set main photo");
        }

        [Authorize(Policy = "RequireShopKeeperRole")]
        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto(DeletePhotoDto photoDto)
        {
            var product = await _uow.ProductRepo.GetProduct(photoDto.Id);
            var photo = product.Photos.SingleOrDefault(p => p.Id == photoDto.PhotoId);

            if (photo == null) return NotFound("There is no findings :)");
            if (photo.IsMain) return BadRequest("Cannot delete main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            product.Photos.Remove(photo);
            
            if (await _uow.Complete()) return Ok("Successfully deleted photo");            

            return BadRequest("Error deleting the photo");
        }
    }
}