using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Customers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.CustomersCQ.Commands
{
    public record AddCustommerCommand(CreateCustomerDto dto): IRequest<Result<CustomersDto >>; 
    public class AddCustomerHandler(IMapper mapper, IUnitOfWork context) : IRequestHandler<AddCustommerCommand, Result<CustomersDto>>
    {
        public async Task<Result<CustomersDto>> Handle(AddCustommerCommand request, CancellationToken ct)
        {
            var existantCustomer = await context.Customers.GetByNameAsync(request.dto.Name, ct);
            if (existantCustomer is not null)
                return Result<CustomersDto>.Failure(false, "El nombre de este usuario ya existe", 409);
            var mapped = mapper.Map<Customers>(request.dto);
            var newCustomer = context.Customers.AddAsync( mapped, ct);
            var response = mapper.Map<CustomersDto>(newCustomer);
            return Result<CustomersDto>.Success(response, 201);
        }
    }
    
    
}
