
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Purchases;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.PurchasesCQ.Queries
{
    public record GetPurchasesQuery(FilterPurchasesDto dto) : IRequest<Result<PaginationList<PurchaseDto>>>;
    public class GetPurchasesHandler(IMapper mapper, IUnitOfWork context): IRequestHandler<GetPurchasesQuery, Result<PaginationList<PurchaseDto>>>
    {
        public async Task<Result<PaginationList<PurchaseDto>>> Handle(GetPurchasesQuery request, CancellationToken ct)
        {
            var response = await context.Purchases.GetPurchasesAsync(request.dto, ct);
            if (!response.Any())
                return Result<PaginationList<PurchaseDto>>.Failure(true, "No hay registros", 200);

            int totalPages = (int)Math.Ceiling((double)response.Count() / request.dto.PageSize);
            var mapped = mapper.Map<List<PurchaseDto>>(response);
            var pagination = new PaginationList<PurchaseDto>(mapped, request.dto.PageNumber, totalPages);
            return Result<PaginationList<PurchaseDto>>.Success(pagination, 200);
        }
    }
}
