using MediatR;
using Microsoft.AspNetCore.Mvc;

using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.InventoryCQ.Queries;
 
namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ApiController
    {
        private readonly IMediator _mediator;
 
        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
 
        [HttpGet]
        public async Task<IActionResult> GetInventory([FromQuery] FilterInventoryDto dto, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetInventoryQuery(dto), ct);
            return HandleResult(response);
        }
 
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInventoryById(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetInventoryByIdQuery(id), ct);
            return HandleResult(response);
        }
 
        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetInventoryByProductId(int productId, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetInventoryByProductIdQuery(productId), ct);
            return HandleResult(response);
        }
 
        [HttpGet("product/name/{productName}")]
        public async Task<IActionResult> GetInventoryByProductName(string productName, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetInventoryByProductNameQuery(productName), ct);
            return HandleResult(response);
        }
    }
}