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
            CreateMap<Categories, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Categories>().ForMember(dest => dest.Id, opt=> opt.Ignore());
            CreateMap<UpdateCategoryDto, Categories>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
