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
    public record GetSalesQuery(FilterSalesDto dto) : IRequest<Result<IEnumerable<SaleDto>>>;
    public class GetSalesHandler(IMapper mapper, IUnitOfWork context) : IRequestHandler<GetSalesQuery, Result<IEnumerable<SaleDto>>>
    {
        public async Task<Result<IEnumerable<SaleDto>>> Handle(GetSalesQuery request, CancellationToken ct)
        {
            var response = await context.Sales.GetSalesAsync(request.dto, ct);
            if (!response.Any())
                return Result<IEnumerable<SaleDto>>.Failure(true, "No hay registros", 200);

            var mapped = mapper.Map<List<SaleDto>>(response);
            return Result<IEnumerable<SaleDto>>.Success(mapped, 200);
        }
    }
}
