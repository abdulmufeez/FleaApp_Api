using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;

namespace FleaApp_Api.Interfaces
{
    public interface ISubCategoryRepo
    {
        void AddSubCategory(SubCategory subCategory);
        void UpdateSubCategory(SubCategory subCategory);
        void DeleteSubCategory(SubCategory subCategory);

        Task<SubCategory> GetSubCategory(int id);
        Task<SubCategoryDto> GetSubCategoryAsync(int id);
        Task<SubCategoryDto> GetSubCategoryAsync(string name);
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync();
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(int categoryId);
        Task<bool> SubCategoryExist(string name);
    }
}