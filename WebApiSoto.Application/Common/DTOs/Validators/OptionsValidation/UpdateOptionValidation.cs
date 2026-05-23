using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WebApiSoto.Application.Common.DTOs.OptionsDtos;

namespace WebApiSoto.Application.Common.DTOs.Validators.OptionsValidation
{
    public class UpdateOptionValidation : AbstractValidator<UpdateOptionDto>
    {
        public UpdateOptionValidation()
        {
            // In update, all fields are optional, but if provided they must be valid
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("The name cannot exceed 100 characters")
                .When(x => x.Name != null);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The price must be greater than 0")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Measurement)
                .MaximumLength(50).WithMessage("The measurement unit cannot exceed 50 characters")
                .When(x => x.Measurement != null);

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("The description cannot exceed 250 characters")
                .When(x => x.Description != null);
        }
    }
}
