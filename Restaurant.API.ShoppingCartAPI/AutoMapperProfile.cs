using AutoMapper;
using Restaurant.API.ShoppingCartAPI.Models;
using Restaurant.API.ShoppingCartAPI.Models.Dto;

namespace Restaurant.API.ShoppingCartAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
            CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
            CreateMap<CartDto, Cart>().ReverseMap();
        }
    }
}
