using AutoMapper;
using GoodHamburger.Api.Application.DTOs.Response.Order;
using GoodHamburger.Api.Domain.Entities;

namespace GoodHamburger.Api.Application.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderResponseDTO>();

       CreateMap<OrderItem, OrderItemDTO>()
                            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));
    }
}