using AutoMapper;
using WebApiSoto.Application.Common.DTOs.Sales;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<CreateSaleDto, WebApiSoto.Domain.Models.Sale>()
                .ForMember(dest => dest.SaleId, opt => opt.Ignore())
                .PreserveReferences();
            CreateMap<CreateSaleDetailDto, WebApiSoto.Domain.Models.SaleDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).
                ForMember(dest=> dest.SalePrice, opt=> opt.Ignore());

            CreateMap<WebApiSoto.Domain.Models.Sale, SaleDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : string.Empty));
            CreateMap<WebApiSoto.Domain.Models.SaleDetail, SaleDetailsDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : string.Empty));
                
        }
    }
}
