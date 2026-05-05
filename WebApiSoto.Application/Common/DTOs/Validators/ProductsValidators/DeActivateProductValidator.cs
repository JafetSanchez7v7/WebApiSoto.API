using FluentValidation;
using WebApiSoto.Application.CQRS.ProductsCQ.Commands;

namespace WebApiSoto.Application.Common.DTOs.Validators.ProductsValidators
{
    public class DeActivateProductValidator : AbstractValidator<DeActivateProductCommand>
    {
        public DeActivateProductValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required").GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
