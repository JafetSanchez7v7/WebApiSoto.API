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
            // 1. Mapeo de Entidad -> DTO de salida (Y su reverso)
            // Como Customers y CustomersDto tienen los mismos nombres (Cedula, CustomerName, CustomerAddress), 
            // el ReverseMap() aquí funciona limpio y sin quejas.
            CreateMap<Customers, CustomersDto>().ReverseMap();

            // 2. Mapeo de DTO de Creación -> Entidad original
            CreateMap<CreateCustomerDto, Customers>()
                // Ignoramos el ID porque lo genera la BD
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())

                // Forzamos que IsActive empiece en true
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))

                // MAOPEOS EXPLICITOS POR NOMBRE DIFERENTE:
                // Mapea 'Name' del DTO a 'CustomerName' de la Entidad
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name))

                // Mapea 'Address' del DTO a 'CustomerAddress' de la Entidad
                .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.Address))

               
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.DNI));

            CreateMap<UpdateCustomerDto, Customers>()
        // Ignoramos el ID porque ya viene trackeado de la BD y no debe cambiar
        .ForMember(dest => dest.CustomerId, opt => opt.Ignore())

        // Mapeos explícitos por nombres diferentes
        .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.Address))
        .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.DNI))

    
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
            srcMember != null &&
            (!(srcMember is string s) || !string.IsNullOrWhiteSpace(s))
        ));

        }
    }
}
