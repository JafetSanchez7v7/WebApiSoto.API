using AutoMapper;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Domain.Models;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderDetail, OrderDetailDto>();
        CreateMap<Order, OrderDto>();

        CreateMap<OrderDetail, OrderDetailDto>()
    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null));

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : null));
    }
}