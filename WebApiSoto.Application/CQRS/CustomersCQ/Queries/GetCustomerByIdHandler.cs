using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Customers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.CustomersCQ.Queries
{
    public record GetCustomerByIdQuery(int Id) : IRequest<Result<CustomersDto>>;
    public class GetCustomerByIdHandler(IMapper mapper, IUnitOfWork _uow) : IRequestHandler<GetCustomerByIdQuery, Result<CustomersDto>>
    {
        public async Task<Result<CustomersDto>> Handle(GetCustomerByIdQuery request, CancellationToken ct)
        {
            var customer = await _uow.Customers.GetByIdAsync(request.Id, ct);
            if(customer is null)
                return Result<CustomersDto>.Failure(false, "Customer not found", 404);
            var mappedCustomer = mapper.Map<CustomersDto>(customer);
            return Result<CustomersDto>.Success(mappedCustomer, 200);
        }
    }
    
    
}
