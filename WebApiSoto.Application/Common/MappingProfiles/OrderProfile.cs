using AutoMapper;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Domain.Models;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        
        CreateMap<Order, OrderDto>().
            ForMember(dest => dest.OrderDetails, OPT => OPT.MapFrom(src => src.OrderDetails.ToList()))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : null));

        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                src.ProductId >= 50000
                    ? (src.PersonalizedProduct != null
                        ? src.PersonalizedProduct.Description ?? $"Producto personalizado #{src.ProductId}"
                        : $"Producto personalizado #{src.ProductId}")
                    : (src.Product != null
                        ? src.Product.ProductName
                        : null)
            ));


    }       
}