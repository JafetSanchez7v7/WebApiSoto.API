using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Users;

namespace WebApiSoto.Application.Common.DTOs.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x).Must(user => !(user.IsOperator && (user.IsAdmin || user.IsGerent)))
                    .WithMessage("An operator cant be an Admin user or a Gerent user.");
           
        }
    }
}
