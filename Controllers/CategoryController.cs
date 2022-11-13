using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Extensions;
using FleaApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [Authorize]
        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryAsync(int id)
        {
            var category = await _uow.CategoryRepo.GetCategoryAsync(id);

            if (category is not null) return category;

            return NotFound("There is no findings :)");
        }

        [Authorize]
        [HttpGet("get-category-by-name/{name}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryAsync(string name)
        {
            var category = await _uow.CategoryRepo.GetCategoryAsync(name);

            if (category is not null) return category;

            return NotFound("There is no findings :)");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            var categories = await _uow.CategoryRepo.GetCategoriesAsync();

            if (categories is not null) return Ok(categories);

            return NotFound("There is no findings :)");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-category/{name}")]
        public async Task<ActionResult> CreateCategory(string name)
        {
            if (!await _uow.CategoryRepo.CategoryExists(name.ToLower()))
            {
                var category = new Category
                {
                    Name = name.ToLower(),
                    AppUserId = User.GetAppUserId()
                };

                _uow.CategoryRepo.AddCategory(category);
                if (await _uow.Complete()) return Ok("Successfully Added");
            }
            else return BadRequest("Name already exists");

            return BadRequest("Error Creating entity");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("update-category")]
        public async Task<ActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _uow.CategoryRepo.GetCategory(updateCategoryDto.Id);
            _mapper.Map(updateCategoryDto, category);
            category.Name = category.Name.ToLower();
            category.AppUserId = User.GetAppUserId();

            _uow.CategoryRepo.UpdateCategory(category);

            if (await _uow.Complete()) return Ok("Successfully updated");

            return BadRequest("Error Updating entity");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _uow.CategoryRepo.GetCategory(id);
            _uow.CategoryRepo.DeleteCategory(category);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }
    }
}