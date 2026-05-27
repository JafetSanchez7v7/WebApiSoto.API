using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.DTOs.Sales;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.SalesCQ.Commands;
using WebApiSoto.Application.CQRS.SalesCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ApiController
    {
        private readonly IMediator _mediator;
        public SalesController(IMediator med)
        {
            _mediator = med;
        }

        [HttpGet]
        public async Task<IActionResult> GetSales([FromQuery] FilterSalesDto dto, CancellationToken ct)
        {
            var query = new GetSalesQuery(dto);
            var result = await _mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var query = new GetSaleByIdQuery(id);
            var result = await _mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDto dto, CancellationToken ct)
        {
            var command = new AddSaleCommand(dto);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return HandleResult(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.SaleId }, result);
        }
    }
}
