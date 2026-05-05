using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Users;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.MappingProfiles
{
    public class UserProfile : Profile
    {
        
        public UserProfile()
        {
            CreateMap<UsersDto, Users>().
                ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).
                ForMember(dest => dest.IsGerent, opt => opt.Ignore()).
                ForMember(dest => dest.IsOperator, opt => opt.Ignore());
                

            CreateMap<Users, UsersDto>().
                 ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src=> src.WhichRole()));

            CreateMap<CreateUserDto, Users>().
                ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password))).
                ForMember(dest => dest.IsGerent, opt => opt.MapFrom(src => src.IsGerent)).
                ForMember(dest => dest.IsOperator, opt => opt.MapFrom(src => src.IsOperator)).
                ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin)).
                ForMember(dest => dest.UserId, opt => opt.Ignore()).
                ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));



        }
    }
}
