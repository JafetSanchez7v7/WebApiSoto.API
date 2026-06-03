using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.DTOs.PersonalizedProduct;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.PersonalizedProductCQ.Commands;
using WebApiSoto.Application.CQRS.PersonalizedProductCQ.Queries;
using static WebApiSoto.Application.CQRS.PersonalizedProductCQ.Queries.GetPersonalizedProductById;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalizedProductsController : ApiController
    {
        private readonly IMediator _mediator;

        public PersonalizedProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var query = new GetPersonalizedProductByIdQuery(id);
            var result = await _mediator.Send(query, ct);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonalizedProductDto dto, CancellationToken ct)
        {
            var command = new CreatePersonalizedProductCommand(dto);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return HandleResult(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.PersonalizedId }, result);
        }
    }
}