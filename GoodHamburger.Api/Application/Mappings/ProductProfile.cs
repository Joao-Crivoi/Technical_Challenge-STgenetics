using AutoMapper;
using GoodHamburger.Api.Domain.Entities;
using GoodHamburger.Api.Application.DTOs.Response.Product;

namespace GoodHamburger.Api.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
       CreateMap<Product, ProductResponseDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
        
    }
}