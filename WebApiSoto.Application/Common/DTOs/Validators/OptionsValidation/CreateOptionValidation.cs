using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WebApiSoto.Application.Common.DTOs.OptionsDtos;

namespace WebApiSoto.Application.Common.DTOs.Validators.OptionsValidation
{

    public class CreateOptionValidation : AbstractValidator<CreateOptionDto>
    {
        public CreateOptionValidation()
        {
            RuleFor(x => x.Name)
          .NotEmpty().WithMessage("The name is required")
          .MaximumLength(100).WithMessage("The name cannot exceed 100 characters");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("The price is required")
                .GreaterThan(0).WithMessage("The price must be greater than 0");

            RuleFor(x => x.Measurement)
                .NotEmpty().WithMessage("The measurement unit is required")
                .MaximumLength(50).WithMessage("The measurement unit cannot exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("The description cannot exceed 250 characters");
        }
    }
}
