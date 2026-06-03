using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.OrdersCQ.Queries
{
    public class GetByIdHandler
    {
        public record GetOrderByIdQuery(int Id) : IRequest<Result<OrderDto>>;

        public class GetOrderByIdHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
        {
            public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken ct)
            {
                var order = await uow.Orders.GetByIdAsync(request.Id, ct);

                if (order is null)
                    return Result<OrderDto>.Failure(false, "Pedido no encontrado", 404);

                return Result<OrderDto>.Success(mapper.Map<OrderDto>(order), 200);
            }
        }

    }
}
