using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Users;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.UserDto.Queries
{
    public record GetUserByIdQuery(int id) : IRequest<Result<UsersDto>>;
    public class GetUserByIdHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<GetUserByIdQuery, Result<UsersDto>> {
        public async Task<Result<UsersDto>>Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var response = await context.User.GetByIdAsync(request.id, ct);
            if (response is null)
                return Result<UsersDto>.Failure(false, "User not found", 404);
            var mapped = mapper.Map<UsersDto>(response);
            return Result<UsersDto>.Success(mapped, 200);

        }
    }
    
    }
            
