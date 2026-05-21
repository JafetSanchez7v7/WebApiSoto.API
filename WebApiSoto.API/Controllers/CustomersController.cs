using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.Models;
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

      

    }
}
