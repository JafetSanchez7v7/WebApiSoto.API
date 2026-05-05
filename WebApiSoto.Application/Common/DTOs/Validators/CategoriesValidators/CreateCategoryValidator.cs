using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.CQRS.CategoriesCQ.Commands;

namespace WebApiSoto.Application.Common.DTOs.Validators.CategoriesValidators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x=>x.CategoryName).NotEmpty().WithMessage("Name must be not null");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description Must be not null");
        }
    }
}
