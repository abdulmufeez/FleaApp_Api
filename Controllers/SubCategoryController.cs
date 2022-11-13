using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Extensions;
using FleaApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class SubCategoryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public SubCategoryController(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [Authorize]
        [HttpGet("get-subcategory-by-id/{id}")]
        public async Task<ActionResult<SubCategoryDto>> GetSubCategoryAsync(int id)
        {
            var subcategory = await _uow.SubCategoryRepo.GetSubCategoryAsync(id);

            if (subcategory is not null) return subcategory;

            return NotFound("There is no findings :)");
        }

        [Authorize]
        [HttpGet("get-subcategory-by-name/{name}")]
        public async Task<ActionResult<SubCategoryDto>> GetSubCategoryAsync(string name)
        {
            var subcategory = await _uow.SubCategoryRepo.GetSubCategoryAsync(name);

            if (subcategory is not null) return subcategory;

            return NotFound("There is no findings :)");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategoryDto>>> GetSubCategoriesAsync()
        {
            var subcategories = await _uow.SubCategoryRepo.GetSubCategoriesAsync();

            if (subcategories is not null) return Ok(subcategories);

            return NotFound("There is no findings :)");
        }

        [AllowAnonymous]
        [HttpGet("get-subcategories-by-categoryId/{id}")]
        public async Task<ActionResult<IEnumerable<SubCategoryDto>>> GetSubCategoriesAsync(int id)
        {
            var subcategories = await _uow.SubCategoryRepo.GetSubCategoriesAsync(id);

            if (subcategories is not null) return Ok(subcategories);

            return NotFound("There is no findings :)");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-subcategory")]
        public async Task<ActionResult> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto)
        {
            if (!await _uow.SubCategoryRepo.SubCategoryExist(createSubCategoryDto.Name.ToLower()))
            {
                var subcategory = _mapper.Map<SubCategory>(createSubCategoryDto);
                subcategory.CreatedAt = DateTime.Now;
                subcategory.AppUserId = User.GetAppUserId();

                _uow.SubCategoryRepo.AddSubCategory(subcategory);
                if (await _uow.Complete()) return Ok("Successfully Added");
            }
            else return BadRequest("Name already exists");   

            return BadRequest("Error Creating entity");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("update-subcategory")]
        public async Task<ActionResult> UpdateSubCategory(UpdateSubCategoryDto updateSubCategoryDto)
        {
            var subcategory = await _uow.SubCategoryRepo.GetSubCategory(updateSubCategoryDto.Id);
            _mapper.Map(updateSubCategoryDto, subcategory);
            subcategory.Name = subcategory.Name.ToLower();
            subcategory.AppUserId = User.GetAppUserId();

            _uow.SubCategoryRepo.UpdateSubCategory(subcategory);

            if (await _uow.Complete()) return Ok("Successfully updated");

            return BadRequest("Error Updating entity");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-subcategory/{id}")]
        public async Task<ActionResult> DeleteSubCategory(int id)
        {
            var subcategory = await _uow.SubCategoryRepo.GetSubCategory(id);
            _uow.SubCategoryRepo.DeleteSubCategory(subcategory);

            if (await _uow.Complete()) return Ok("Successfully Deleted");

            return BadRequest("Error deleting entity");
        }
    }
}