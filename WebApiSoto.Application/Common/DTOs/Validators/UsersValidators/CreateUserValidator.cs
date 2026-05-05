using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Users;

namespace WebApiSoto.Application.Common.DTOs.Validators.UsersValidators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x=> x.UserName).NotEmpty().WithMessage("User name is required")
                                    .MaximumLength(50).WithMessage("User name must be at most 50 characters");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").
                                    MinimumLength(6).WithMessage("Password must be at least 6 characters").
                                    MaximumLength(16).WithMessage("Password must be at most 16 characters");
            RuleFor(x => x).Must(user => !(user.IsOperator && (user.IsAdmin || user.IsGerent)))
                           .WithMessage("An operator cant be an Admin user or a Gerent user.");
        }
    }
}
 