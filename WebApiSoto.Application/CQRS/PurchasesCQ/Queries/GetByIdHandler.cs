using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Purchases;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.PurchasesCQ.Queries
{
    public record GetPurchaseByIdQuery(int id): IRequest<Result<PurchaseDto>>;
    public class GetByIdHandler(IUnitOfWork context, IMapper mapper): IRequestHandler<GetPurchaseByIdQuery, Result<PurchaseDto>>
    {
        public async Task<Result<PurchaseDto>>Handle(GetPurchaseByIdQuery request, CancellationToken ct)
        {
            var response = await context.Purchases.GetByIdAsync(request.id, ct);
            if (response == null)
                return Result<PurchaseDto>.Failure(false, "No se encontro la compra consultada", 404);
            var mapped = mapper.Map<PurchaseDto>(response);

            return Result<PurchaseDto>.Success(mapped, 200);
        }
    }
}
