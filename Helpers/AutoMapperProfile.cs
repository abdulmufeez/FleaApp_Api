using AutoMapper;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;

namespace FleaApp_Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Market, MarketDto>()
                .ForMember(d => d.MainPhotoUrl, o => o.MapFrom(s => s.Photos.SingleOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points))
                .ForMember(d => d.Shops, o => o.MapFrom(s => s.Shop.Where(x => x.MarketId == s.Id)));
            CreateMap<MarketDto, Market>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            CreateMap<CreateMarketDto, Market>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));            
            CreateMap<UpdateMarketDto, Market>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            
            CreateMap<GeoLocationDto, GeoLocation>();       
            CreateMap<GeoLocation, GeoLocationDto>();
            CreateMap<Photo, PhotoDto>();

            CreateMap<Shop, ShopDto>()
                .ForMember(d => d.MainPhotoUrl, o => o.MapFrom(s => s.Photos.SingleOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));               
            CreateMap<ShopDto, Shop>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            CreateMap<CreateShopDto, Shop>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));            
            CreateMap<UpdateShopDto, Shop>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points)); 

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();             
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.Category.Id));
            CreateMap<SubCategoryDto, SubCategory>();                
            CreateMap<CreateSubCategoryDto, SubCategory>();
            CreateMap<UpdateSubCategoryDto, SubCategory>();

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.MainPhotoUrl, o => o.MapFrom(s => s.Photos.SingleOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.ShopId, o => o.MapFrom(s => s.Shop.Id))
                .ForMember(d => d.SubCategoryId, o => o.MapFrom(s => s.SubCategory.Id));
            CreateMap<ProductDto, Product>();                
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}