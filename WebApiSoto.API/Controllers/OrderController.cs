using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.OrdersCQ;
using WebApiSoto.Application.CQRS.OrdersCQ.Queries;
using static WebApiSoto.Application.CQRS.OrdersCQ.Commands.CreateOrderHandler;

using static WebApiSoto.Application.CQRS.OrdersCQ.Queries.GetByDateRangeHandler;
using static WebApiSoto.Application.CQRS.OrdersCQ.Queries.GetByIdHandler;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] FilterOrderDto dto, CancellationToken ct)
        {
            var query = new GetOrdersQuery(dto);
            var response = await _mediator.Send(query, ct);
            return HandleResult(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpGet("byDate")]
        public async Task<IActionResult> GetByDate([FromQuery] FilterOrderDto dto, CancellationToken ct)
        {
            if (dto.StartDate is null || dto.EndDate is null)
                return BadRequest("Las fechas de inicio y fin son requeridas");

            var result = await _mediator.Send(new GetOrdersByDateQuery(dto.StartDate.Value, dto.EndDate.Value, dto), ct);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
        {
            var command = new CreateOrderCommand(dto);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return HandleResult(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.OrderId }, result);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand(dto), ct);
            return HandleResult(result);
        }
    }
}