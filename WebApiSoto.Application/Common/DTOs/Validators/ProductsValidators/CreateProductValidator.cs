using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Products;

namespace WebApiSoto.Application.Common.DTOs.Validators.ProductsValidators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName is required");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0");
            RuleFor(x => x.SupplierId).GreaterThan(0).WithMessage("SupplierId must be greater than 0");
        }
    }
}
