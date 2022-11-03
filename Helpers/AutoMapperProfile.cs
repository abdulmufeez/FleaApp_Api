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
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            CreateMap<MarketDto, Market>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            CreateMap<CreateMarketDto, Market>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));            
            CreateMap<UpdateMarketDto, Market>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            
            CreateMap<GeoLocationDto, GeoLocation>();       
            CreateMap<GeoLocation, GeoLocationDto>();

            CreateMap<Shop, ShopDto>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));               
            CreateMap<ShopDto, Shop>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));
            CreateMap<CreateShopDto, Shop>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));            
            CreateMap<UpdateShopDto, Shop>()
                .ForMember(d => d.Points, o => o.MapFrom(s => s.Points));               
        }
    }
}