using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class LoginValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginValidator() {
            RuleFor(l => l.UserName).NotEmpty().WithMessage("User Name is required");
            RuleFor(l => l.Password).NotEmpty().MinimumLength(6).WithMessage("Password must have more than six charachters");
        }

    }
}
