using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.DTOs.Customers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.CustomersCQ.Commands;
using WebApiSoto.Application.CQRS.CustomersCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ApiController
    {
        private readonly IMediator _mediator;
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin,Gerent,Operator")]
        [HttpGet]
        public async Task<IActionResult>GetCustomersAsync([FromQuery]FiltersDto filters, CancellationToken ct, IValidator<FiltersDto> validator)
        {
            var validation = validator.Validate(filters);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                return BadRequest(new { Errors = errors });
            }
            var response = await _mediator.Send(new GetAllCustomerQuery(filters), ct);
            return HandleResult(response);
        }

        [Authorize(Roles = "Admin,Gerent,Operator")]

        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetCustomerByIdQuery(id), ct);
            return HandleResult(response); 
        }
        [Authorize(Roles = "Admin,Gerent,Operator")]

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDto dto, [FromServices] IValidator<CreateCustomerDto> validator, CancellationToken ct)
        {
            var validation = validator.Validate(dto);
            if(!validation.IsValid)
            {
                var errors = validation.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Errors = errors });
            }
            var response = await _mediator.Send(new AddCustommerCommand(dto), ct);
            if (!response.IsSuccess)
                return HandleResult(response);
            else{
                return CreatedAtAction(nameof(GetById), new { id = response.Value.CustomerId }, response);
            }
        }

        [Authorize(Roles = "Admin,Gerent,Operator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto , [FromServices] IValidator<UpdateCustomerDto> validator, CancellationToken ct)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage).ToList());
            var response = await _mediator.Send(new UpdateCustomerCommand(id, dto), ct);
            return HandleResult(response);
        }

        [Authorize(Roles = "Admin,Gerent,Operator")]
        [HttpPatch("{id}")]
        public async Task<IActionResult>DeactiveCustomer(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new DeactivateCustomerCommand(id), ct);
            return HandleResult(response);
        }
  

    }
}
