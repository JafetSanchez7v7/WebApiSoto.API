using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Users;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.UserDto.Commands
{
    public record AddUserCommand(CreateUserDto dto) : IRequest<Result<UsersDto>>;
    public class AddUserCommandHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<AddUserCommand,Result<UsersDto>>
    {
        public async Task<Result<UsersDto>>Handle(AddUserCommand request, CancellationToken ct)
        {
            var existentName = await context.User.GetByNameAsync(request.dto.UserName, ct);
            if (existentName != null)
                return Result<UsersDto>.Failure(false, "Nombre de usuario no disponible", 409);

            var newUser = await context.User.AddAsync(mapper.Map<Domain.Models.Users>(request.dto), ct);
            await context.SaveChangesAsync(ct);
            var userDto = mapper.Map<UsersDto>(newUser);
            return Result<UsersDto>.Success(userDto, 201);
        }
        

        
    }
}
