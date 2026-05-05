using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Customers;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customers, CustomersDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customers>().ForMember(src => src.CustomerId, opt => opt.Ignore()).
                ForMember(src => src.IsActive, opt => opt.MapFrom(src=> true));
        }
    }
}
