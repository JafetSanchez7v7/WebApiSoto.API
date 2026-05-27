using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Purchases;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<CreatePurchaseDto, Purchase>().
                ForMember(dest => dest.PurchaseId, opt => opt.Ignore()).PreserveReferences();
            CreateMap<CreatePurchaseDetailDto, PurchaseDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Purchase, PurchaseDto>().ForMember(dest=> dest.SupplierName, opt=> opt.MapFrom(src=>src.Suppliers != null? src.Suppliers.Name : string.Empty));
            CreateMap<PurchaseDetail, PurchaseDetailsDto>().ForMember(dest=>dest.ProductName, opt=> opt.MapFrom(src=>src.Products != null ? src.Products.ProductName : string.Empty));
        }
    }
}
