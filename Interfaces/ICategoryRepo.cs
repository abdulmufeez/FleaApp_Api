using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;

namespace FleaApp_Api.Interfaces
{
    public interface ICategoryRepo
    {
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);

        Task<Category> GetCategory(int id);
        Task<CategoryDto> GetCategoryAsync(int id);        
        Task<CategoryDto> GetCategoryAsync(string name);        
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<bool> CategoryExists(string name);
    }
}