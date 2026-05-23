using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.Application.Common.DTOs.Validators.OptionsValidation
{
    public class FIlterOptionsValidation : AbstractValidator<FIlterOptionsDto>
    {
        public FIlterOptionsValidation()
        {
            RuleFor(x=>x.PriceGreaterThan).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
}
