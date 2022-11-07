using AutoMapper;
using AutoMapper.QueryableExtensions;
using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CategoryRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.AddAsync(category);
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .AsQueryable();
            
            return await categories.ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryAsync(int id)
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)                
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CategoryDto> GetCategoryAsync(string name)
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Name.Contains(name))
                .SingleOrDefaultAsync();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task<bool> CategoryExists(string name)
            => await _context.Categories.AnyAsync(c => c.Name.Contains(name));
    }
}