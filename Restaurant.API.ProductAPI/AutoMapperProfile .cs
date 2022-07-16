using AutoMapper;
using Restaurant.API.ProductAPI.Models;
using Restaurant.API.ProductAPI.Models.Dto;

namespace Restaurant.API.ProductAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
