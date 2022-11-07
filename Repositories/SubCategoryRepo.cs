using AutoMapper;
using AutoMapper.QueryableExtensions;
using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class SubCategoryRepo : ISubCategoryRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SubCategoryRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.AddAsync(subCategory);
        }

        public void DeleteSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Remove(subCategory);
        }

        public async Task<SubCategory> GetSubCategory(int id)
        {
            return await _context.SubCategories
                .Include(x => x.Category)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync()
        {
            var subCategories = _context.SubCategories
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await subCategories.ToListAsync();
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Update(subCategory);
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(int categoryId)
        {
            var subCategories = _context.SubCategories
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await subCategories.Where(s => s.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<SubCategoryDto> GetSubCategoryAsync(int id)
        {
            return await _context.SubCategories
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)            
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SubCategoryDto> GetSubCategoryAsync(string name)
        {
            return await _context.SubCategories
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Name.Contains(name))
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SubCategoryExist(string name)
            => await _context.SubCategories.AnyAsync(s => s.Name.Contains(name));
    }
}