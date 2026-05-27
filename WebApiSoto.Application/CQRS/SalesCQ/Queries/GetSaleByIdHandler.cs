using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Sales;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.SalesCQ.Queries
{
    public record GetSaleByIdQuery(int id) : IRequest<Result<SaleDto>>;
    public class GetSaleByIdHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<GetSaleByIdQuery, Result<SaleDto>>
    {
        public async Task<Result<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken ct)
        {
            var response = await context.Sales.GetByIdAsync(request.id, ct);
            if (response == null)
                return Result<SaleDto>.Failure(false, "No se encontro la venta consultada", 404);

            var mapped = mapper.Map<SaleDto>(response);
            return Result<SaleDto>.Success(mapped, 200);
        }
    }
}
