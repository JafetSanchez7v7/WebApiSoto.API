using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Validators.UsersValidators;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.UsersCQ.Commands
{
    public record UpdatePasswordCommand(int id, UpdatePasswordDto dto) : IRequest<Result<Unit>>;
    public  class UpdatePasswordHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePasswordCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(UpdatePasswordCommand request, CancellationToken ct)
        {
            var userToUpdate = await unitOfWork.User.GetToUpdateAsync(request.id, ct);
            if (userToUpdate == null)
                return Result<Unit>.Failure(false, "User not found", 404);
            var IsValid = BCrypt.Net.BCrypt.Verify(request.dto.Password, userToUpdate.PasswordHash);
            if (!IsValid)
                return Result<Unit>.Failure(false, "Password is incorrect", 400);
            userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.dto.Password);
            await unitOfWork.SaveChangesAsync(ct);
            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}
