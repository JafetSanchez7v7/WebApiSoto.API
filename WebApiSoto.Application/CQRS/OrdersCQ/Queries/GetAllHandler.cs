using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.OrdersCQ.Queries
{
    public record GetOrdersQuery(FilterOrderDto dto) : IRequest<Result<PaginationList<OrderDto>>>;
    public class GetAllHandler(IUnitOfWork _context, IMapper mapper) : IRequestHandler<GetOrdersQuery, Result<PaginationList<OrderDto>>>
    {
        public async Task<Result<PaginationList<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken ct)
        {
            var lista = await _context.Orders.GetAll(request.dto, ct);
            if (!lista.Any())
                return Result<PaginationList<OrderDto>>.Failure(true, "no hay registros", 200);

            var mapped = mapper.Map<IEnumerable<OrderDto>>(lista).ToList();

            var totalRegisters = await _context.Orders.CountAsync(request.dto,ct);
            var totalPages = (int)Math.Ceiling(totalRegisters / (double)request.dto.PageSize);
            var pagination = new PaginationList<OrderDto>(mapped, request.dto.PageNumber, totalPages, totalRegisters);

            return Result<PaginationList<OrderDto>>.Success(pagination, 200);
        }
    }
}
