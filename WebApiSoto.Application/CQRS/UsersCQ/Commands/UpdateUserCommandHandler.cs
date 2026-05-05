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

namespace WebApiSoto.Application.CQRS.Users.Commands
{
    public record UpdateUserCommand(int id, UpdateUserDto dto) : IRequest<Result<Unit>>;
    public class UpdateUserCommandHandler(IUnitOfWork context) : IRequestHandler<UpdateUserCommand, Result<Unit>>
    {
        public async Task<Result<Unit>>Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var userToUpdate = await context.User.GetToUpdateAsync(request.id, ct);
            if (userToUpdate == null)
                return Result<Unit>.Failure(false, "User not found", 404);
           
            userToUpdate.UserName = request.dto.UserName;
            userToUpdate.IsActive = request.dto.IsActive;
            userToUpdate.IsAdmin = request.dto.IsAdmin;
            userToUpdate.IsGerent = request.dto.IsGerent;
            userToUpdate.IsOperator = request.dto.IsOperator;

            await context.SaveChangesAsync(ct);

            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}
