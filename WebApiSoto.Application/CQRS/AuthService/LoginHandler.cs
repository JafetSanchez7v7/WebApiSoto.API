using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.AuthService
{
    public record LoginCommand(LoginRequestDto dto) : IRequest<Result<LoginResponse>>;
    public class LoginHandler(IUnitOfWork context, ITokenProvider token) : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await context.User.GetByNameAsync(request.dto.UserName, ct);
            if (user == null)
                return Result<LoginResponse>.Failure(false, "User or Password are incorrect", 401);
            
            var IsValidPassword = BCrypt.Net.BCrypt.Verify(request.dto.Password, user.PasswordHash);
            if (!IsValidPassword) 
                return Result<LoginResponse>.Failure(false, "User or Password are incorrect", 401);
             var JWT = token.GetToken(user);

            var response = new LoginResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Token = JWT,
                Role = user.WhichRole()
            };

            return Result<LoginResponse>.Success(response, 200);
        }
    }
}
