using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.CQRS.SuppliersCQ.Commands;

namespace WebApiSoto.Application.Common.DTOs.Validators.SuppliersValidators
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierDto>
    {
        public CreateSupplierValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be not null");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location must be not null");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone must be not null");
        }
    }
}
