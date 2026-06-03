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
    public class GetByDateRangeHandler
    {
        public record GetOrdersByDateQuery(DateTime Start, DateTime End, FilterOrderDto Dto) : IRequest<Result<PaginationList<OrderDto>>>;

        public class GetOrdersByDateHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<GetOrdersByDateQuery, Result<PaginationList<OrderDto>>>
        {
            public async Task<Result<PaginationList<OrderDto>>> Handle(GetOrdersByDateQuery request, CancellationToken ct)
            {
                var orders = await uow.Orders.GetByDateRangeAsync(request.Start, request.End, request.Dto, ct);

                if (!orders.Any())
                    return Result<PaginationList<OrderDto>>.Failure(true, "No se encontraron pedidos en ese rango de fechas", 200);

                var totalPages = (int)Math.Ceiling((double)orders.Count() / request.Dto.PageSize);
                var mapped = mapper.Map<List<OrderDto>>(orders);
                var totalRegisters = await uow.Orders.CountAsync(ct);
                var pagination = new PaginationList<OrderDto>(mapped, request.Dto.PageNumber, totalPages, totalRegisters);

                return Result<PaginationList<OrderDto>>.Success(pagination, 200);
            }
        }
    }
}
