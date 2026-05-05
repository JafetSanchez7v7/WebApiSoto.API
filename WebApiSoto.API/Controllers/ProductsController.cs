using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApiSoto.Application.Common.DTOs.Products;
using WebApiSoto.Application.CQRS.ProductsCQ.Commands;
using WebApiSoto.Application.CQRS.ProductsCQ.Queries;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] FiltersDto dto, IValidator<FiltersDto> validator, CancellationToken ct)
        {
            var validation = validator.Validate(dto);
            if (validation.Errors.Any())
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new GetAllProductsQuery(dto), ct);
            return HandleResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetProductByIdQuery(id), ct);
            return HandleResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto dto, CancellationToken ct, [FromServices] IValidator<CreateProductDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new AddProductCommand(dto), ct);
            if (!response.IsSuccess)
                return HandleResult(response);

            return CreatedAtAction(nameof(GetProductById), new { id = response.Value.ProductId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto, CancellationToken ct, [FromServices] IValidator<UpdateProductDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new UpdateProductCommand(id, dto), ct);
            return HandleResult(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DeActivateProduct(int id, [FromBody] DeActivateProductCommand command, [FromServices] IValidator<DeActivateProductCommand> validator, CancellationToken ct)
        {
            var validation = validator.Validate(command);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new DeActivateProductCommand(id), ct);
            return HandleResult(response);
        }
    }
}
