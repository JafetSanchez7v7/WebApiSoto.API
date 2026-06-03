using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Invoices;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDto>().ForMember(dest=> dest.InvoiceDetails, opt=> opt.MapFrom(src=> src.InvoiceDetails));
            CreateMap<InvoiceDetail, InvoiceDetailsDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

            CreateMap<Sale, Invoice>()
            // Pasamos el ID que generó la base de datos de la venta al SaleId de la factura
            .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.SaleId))
            // El total acumulado de la venta pasa a ser el monto total de la factura
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.SaleTotal))
          
            .ForMember(dest => dest.InvoiceDetails, opt => opt.MapFrom(src => src.SaleDetails))
           
            .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
            
            .ForMember(dest => dest.IsPrinted, opt => opt.MapFrom(src=> false))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src=> src.SaleDate))
            .ForMember(dest => dest.PrintedDate, opt => opt.Ignore());

            // Mapeo de los hijos: Detalle de Venta a Detalle de Factura
            CreateMap<SaleDetail, InvoiceDetail>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                // LineAmount en la venta pasa a ser LineTotal en la factura
                .ForMember(dest => dest.LineTotal, opt => opt.MapFrom(src => src.LineAmount))
                // Ignoramos las llaves primarias y navegaciones para evitar conflictos de rastreo en EF Core
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
                .ForMember(dest => dest.Invoice, opt => opt.Ignore());

        }
    }
}
