using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Categories, CategoryDto>()
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon));

            CreateMap<CreateCategoryDto, Categories>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

            CreateMap<UpdateCategoryDto, Categories>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        }
    }
}
