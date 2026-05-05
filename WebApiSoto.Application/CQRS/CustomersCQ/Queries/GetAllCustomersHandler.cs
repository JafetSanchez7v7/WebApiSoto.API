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
    public record GetAllCustomerQuery(FiltersDto dto) : IRequest<Result<PaginationList<CustomersDto>>>;
    public class GetAllCustomersHandler(IMapper mapper, IUnitOfWork _uow) : IRequestHandler<GetAllCustomerQuery, Result<PaginationList<CustomersDto>>>
    {
        public async Task<Result<PaginationList<CustomersDto>>> Handle(GetAllCustomerQuery request, CancellationToken ct)
        {
            var response = await _uow.Customers.GetCustomersAsync(request.dto, ct);
            if (!response.Any())
                return Result<PaginationList<CustomersDto>>.Failure(true, "No Registers", 200);

            var  totalPages = (int)Math.Ceiling(response.Count() / (double)request.dto.PageSize);
            var mapping = mapper.Map<List<CustomersDto>>(response);

            var PaginationList = new PaginationList<CustomersDto>(mapping, request.dto.PageSize, totalPages);

            return Result<PaginationList<CustomersDto>>.Success(PaginationList, 200);

        }
    }
}