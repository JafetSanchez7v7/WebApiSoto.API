using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.CQRS.SuppliersCQ.Commands;

namespace WebApiSoto.Application.Common.DTOs.Validators.SuppliersValidators
{
    public class DeActivateSupplierValidator : AbstractValidator<DeActivateSupplierCommand>
    {
        public DeActivateSupplierValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required").GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
