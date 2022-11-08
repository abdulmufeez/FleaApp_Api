using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Helpers;

namespace FleaApp_Api.Interfaces
{
    public interface IProductRepo
    {
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<ProductDto> GetProductAsync(string name);
        Task<ProductDto> GetProductAsync(int id);
        Task<Product> GetProduct(int id);

        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);
    }
}