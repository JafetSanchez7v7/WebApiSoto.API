using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.SuppliersCQ.Queries
{
    public record GetAllSuppliersQuery(FiltersDto dto) : IRequest<Result<PaginationList<SupplierDto>>>;

    public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, Result<PaginationList<SupplierDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllSuppliersHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<PaginationList<SupplierDto>>> Handle(GetAllSuppliersQuery request, CancellationToken ct)
        {
            var response = await _uow.Supplier.GetSuppliersAsync(request.dto, ct);
            if (!response.Any())
                return Result<PaginationList<SupplierDto>>.Failure(true, "No Registers", 200);

            var totalPages = (int)Math.Ceiling((double)response.Count() / request.dto.PageSize);
            var mapped = _mapper.Map<List<SupplierDto>>(response);
            var pagination = new PaginationList<SupplierDto>(mapped, request.dto.PageNumber, totalPages);
            return Result<PaginationList<SupplierDto>>.Success(pagination, 200);
        }
    }
}
