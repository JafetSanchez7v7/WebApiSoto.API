using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Suppliers, SupplierDto>().ReverseMap();
            CreateMap<CreateSupplierDto, Suppliers>().ForMember(dest => dest.SupplierId, opt => opt.Ignore());
            CreateMap<UpdateSupplierDto, Suppliers>().ForMember(dest => dest.SupplierId, opt => opt.Ignore());
        }
    }
}
