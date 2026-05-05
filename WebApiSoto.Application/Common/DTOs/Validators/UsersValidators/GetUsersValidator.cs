using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.Application.Common.DTOs.Validators.UsersValidators
{
    public class GetUsersValidator : AbstractValidator<FiltersDto>
    {
        public GetUsersValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name must be at most 50 characters");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
        }
    }
}
