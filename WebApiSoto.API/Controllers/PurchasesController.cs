using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using WebApiSoto.Application.Common.DTOs.Purchases;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.PurchasesCQ.Commands;
using WebApiSoto.Application.CQRS.PurchasesCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ApiController
    {
        private readonly IMediator _mediator;
        public PurchasesController(IMediator med)
        {
            _mediator = med;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases(FilterPurchasesDto dto, CancellationToken ct)
        {
            var query = new GetPurchasesQuery(dto);
            var result = await _mediator.Send(query, ct);
            return HandleResult(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var query = new GetPurchaseByIdQuery(id);
            var result = await _mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseDto dto, CancellationToken ct)
        {
            var command = new AddPurchaseCommand(dto);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return HandleResult(result);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Value.PurchaseId }, result);
        }


    }
}
