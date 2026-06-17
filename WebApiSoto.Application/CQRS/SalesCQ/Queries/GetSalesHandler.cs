using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Sales;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.SalesCQ.Queries
{
    public record GetSalesQuery(FilterSalesDto dto) : IRequest<Result<PaginationList<SaleDto>>>;
    public class GetSalesHandler(IMapper mapper, IUnitOfWork context) : IRequestHandler<GetSalesQuery, Result<PaginationList<SaleDto>>>
    {
        public async Task<Result<PaginationList<SaleDto>>> Handle(GetSalesQuery request, CancellationToken ct)
        {
            var response = await context.Sales.GetSalesAsync(request.dto, ct);
            if (!response.Any())
                return Result<PaginationList<SaleDto>>.Failure(true, "No hay registros", 200);

           
            var mapped = mapper.Map<List<SaleDto>>(response);
            var count = await context.Sales.CountAsync(request.dto, ct);

            var totalPages = (int)Math.Ceiling(count / (double)request.dto.PageSize);

            var pagination= new PaginationList<SaleDto>(mapped,request.dto.PageNumber,totalPages, count);

            return Result<PaginationList<SaleDto>>.Success(pagination, 200);

        }
    }
}
