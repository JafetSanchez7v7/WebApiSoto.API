using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebApiSoto.Application.Common.DTOs.OptionsDtos;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<Option , OptionDto>().ReverseMap();
            CreateMap<CreateOptionDto, Option>().ForMember(src=> src.OptionId, opt=>opt.Ignore());
        }
    }
}
