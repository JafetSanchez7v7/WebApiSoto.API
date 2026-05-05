using FluentValidation;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.CQRS.CategoriesCQ.Commands;

namespace WebApiSoto.Application.Common.DTOs.Validators.CategoriesValidators
{
    public class DeActivateCategoryValidator: AbstractValidator<DeactivateCategoryCommand>
    {
        public DeActivateCategoryValidator()
        {
            RuleFor(x=>x.Id).NotNull().WithMessage("Id is required").GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
