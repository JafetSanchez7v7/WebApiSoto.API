using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebApiSoto.Application.Common.DTOs.PersonalizedProduct;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class PersonalizedProductProfile: Profile
    {
        public PersonalizedProductProfile()
        {
            CreateMap<Personalization, PersonalizationDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Option != null ? src.Option.Name : string.Empty))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => src.SalePrice ?? 0))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal ?? 0));

            CreateMap<PersonalizedProduct, PersonalizedProductDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId ?? 0))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : string.Empty))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId ?? 0))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => src.SalePrice ?? 0))
                .ForMember(dest => dest.PersonalizationDetails, opt => opt.MapFrom(src => src.Personalizations));


        }
    }
}
