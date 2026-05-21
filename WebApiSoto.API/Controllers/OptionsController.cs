using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.OptionsCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ApiController
    {
        private readonly IMediator mediator;
        public OptionsController(IMediator med)
        {
            mediator = med;
        }

        [HttpGet]
        public async Task<IActionResult> GetOptions([FromQuery] FIlterOptionsDto dto, CancellationToken ct, [FromServices] IValidator<FIlterOptionsDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }
            var response = await mediator.Send(new GetOptionsQuery(dto), ct);
            return HandleResult(response);
        }
    }
}
