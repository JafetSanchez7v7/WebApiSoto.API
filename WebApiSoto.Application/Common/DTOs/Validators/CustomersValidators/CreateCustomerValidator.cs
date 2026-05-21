using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Customers;

namespace WebApiSoto.Application.Common.DTOs.Validators.CustomersValidators
{
    public  class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede pasar de 100 caracteres.");

            // 2. Cédula de Identidad (DNI) - Soporta limpia (15 caracteres) o con guiones (19 caracteres)
            // Ejemplo: 0012805060001A o 001-280506-0001A
            RuleFor(x => x.DNI)
                .NotEmpty().WithMessage("La cédula es obligatoria.")
                .Matches(@"^\d{3}-?\d{6}-?\d{4}[A-Z]$").WithMessage("El formato de cédula no es válido (Ej: 001-000000-0000A).");

            // 3. Dirección
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(250).WithMessage("La dirección es muy larga.");

            // 4. Ciudad / Municipio
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad es obligatoria.");

            // 5. Teléfono de Nicaragua (8 dígitos, opcional con guion en medio o código de área +505)
            // Acepta: 88888888, 8888-8888 o con +505
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("El número de teléfono es obligatorio.")
                .Matches(@"^(\+505)?\s?[2578]\d{3}-?\d{4}$").WithMessage("El número de teléfono debe ser válido en Nicaragua (8 dígitos).");
        }
    }
    }

