using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.CQRS.UsersCQ.Commands;

namespace WebApiSoto.Application.Common.DTOs.Validators.UsersValidators
{
    public class DeActivateValidator : AbstractValidator<DeActivateUserCommand>
    {
        public DeActivateValidator()
        {
            RuleFor(d=>d.id).NotEmpty().WithMessage("Id Is Required");
            
        }
    }
}
