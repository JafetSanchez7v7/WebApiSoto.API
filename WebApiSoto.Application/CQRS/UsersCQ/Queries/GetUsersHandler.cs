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
    public record  GetUserQuery(FiltersDto dto) : IRequest<Result<PaginationList<UsersDto>>>;

    public class GetUserHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<GetUserQuery, Result<PaginationList<UsersDto>>> {
    
        public async Task<Result<PaginationList<UsersDto>>> Handle(GetUserQuery request, CancellationToken ct)
        {
           var users = await context.User.GetAsync(request.dto, ct);
           if (!users.Any())
                return Result<PaginationList<UsersDto>>.Failure(true, "No users found", 200);

            var totalCount = await context.User.CountAsync(request.dto, ct);
            var totalPages = (int)Math.Ceiling((double)totalCount / request.dto.PageSize);

            var mapping = mapper.Map<List<UsersDto>>(users);

            return Result<PaginationList<UsersDto>>.Success(new PaginationList<UsersDto>(mapping, request.dto.PageNumber, totalPages, totalCount), 200);
        }
    }

}
