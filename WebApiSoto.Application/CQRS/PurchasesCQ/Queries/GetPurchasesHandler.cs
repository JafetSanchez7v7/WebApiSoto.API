
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

            var totalCount = await context.Purchases.CountAsync(request.dto, ct);
            var pageSize = request.dto.PageSize > 0 ? request.dto.PageSize : 8;
            int totalPages = (int)Math.Ceiling((double)totalCount / (double)pageSize);
            var mapped = mapper.Map<List<PurchaseDto>>(response);
            var pagination = new PaginationList<PurchaseDto>(mapped, request.dto.PageNumber, totalPages, totalCount);
            return Result<PaginationList<PurchaseDto>>.Success(pagination, 200);
        }
    }
}
