using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.UsersCQ.Commands
{
    public record DeActivateUserCommand(int id): IRequest<Result<Unit>>;
    public class DeActivateUserHandler(IUnitOfWork context): IRequestHandler<DeActivateUserCommand,Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(DeActivateUserCommand request, CancellationToken ct)
        {
            var userToUpdate = await context.User.GetByIdAsync(request.id, ct);
            if (userToUpdate == null)
                return Result<Unit>.Failure(false, "No Users Found", 404);
            if(!userToUpdate.IsActive)
                return Result<Unit>.Failure(false, "User is already deactivated", 400);
            
            await context.User.DeactivateAsync(request.id, ct);

            await context.SaveChangesAsync(ct);

            return Result<Unit>.Success(Unit.Value, 204);
        
        }
    }
}
