using AutoMapper;
using AutoMapper.QueryableExtensions;
using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Helpers;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products
                .Include(x => x.Shop)
                .Include(x => x.Photos)
                .Include(x => x.SubCategory)
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<ProductDto> GetProductAsync(string name)
        {
            return await _context.Products
                .Include(x => x.Photos)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .Where(p => p.Name.Contains(name))
                .SingleOrDefaultAsync();            
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            return await _context.Products
                .Include(x => x.Photos)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id);                
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams)
        {
            var products = _context.Products
                .Include(x => x.Shop)
                .Include(x => x.Photos)
                .Include(x => x.SubCategory)
                .AsQueryable();

            if (productParams.SearchByShopId > 0)
                products = products.Where(p => p.ShopId == productParams.SearchByShopId);
            
            if (productParams.SearchBySubCategoryId > 0)
                products = products.Where(p => p.SubCategoryId == productParams.SearchBySubCategoryId);

            if (!string.IsNullOrEmpty(productParams.Search))
                products = products.Where(p => p.Name.Contains(productParams.Search));

            products = productParams.OrderBy switch
            {
                "Recently Added" => products.OrderByDescending(p => p.CreatedAt),
                _ => products.OrderBy(p => p.CreatedAt)
            };

            return await PagedList<ProductDto>.CreateAsync(
                products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
                    productParams.PageNumber, productParams.PageSize
            );
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
    }
}