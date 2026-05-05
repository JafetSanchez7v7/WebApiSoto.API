using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.SuppliersCQ.Commands;
using WebApiSoto.Application.CQRS.SuppliersCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ApiController
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] FiltersDto dto, IValidator<FiltersDto> validator, CancellationToken ct)
        {
            var validation = validator.Validate(dto);
            if (validation.Errors.Any())
            {
                var errors = validation.Errors.Select(c => c.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new GetAllSuppliersQuery(dto), ct);
            return HandleResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetSupplierByIdQuery(id), ct);
            return HandleResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] CreateSupplierDto dto, CancellationToken ct, [FromServices] IValidator<CreateSupplierDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new AddSupplierCommand(dto), ct);
            if (!response.IsSuccess)
                return HandleResult(response);

            return CreatedAtAction(nameof(GetSupplierById), new { id = response.Value.SupplierId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDto dto, CancellationToken ct, [FromServices] IValidator<UpdateSupplierDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new UpdateSupplierCommand(id, dto), ct);
            return HandleResult(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DeActivateSupplier(int id, [FromBody] DeActivateSupplierCommand command, [FromServices] IValidator<DeActivateSupplierCommand> validator, CancellationToken ct)
        {
            var validation = validator.Validate(command);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new DeActivateSupplierCommand(id), ct);
            return HandleResult(response);
        }
    }
}
