using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.DTOs.OptionsDtos;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.OptionsCQ.Commands;
using WebApiSoto.Application.CQRS.OptionsCQ.Queries;
using static WebApiSoto.Application.CQRS.OptionsCQ.Queries.GetOptionbyidHeandler;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOptionById(int id, CancellationToken ct)
        {
            var response = await mediator.Send(new GetOptionByIdQuery(id), ct);
            return HandleResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOption(
          [FromBody] CreateOptionDto dto,
          CancellationToken ct,
         [FromServices] IValidator<CreateOptionDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var response = await mediator.Send(new CreateOptionCommand(dto), ct);
            return HandleResult(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOption(
            int id,
            [FromBody] UpdateOptionDto dto,
            CancellationToken ct,
            [FromServices] IValidator<UpdateOptionDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var response = await mediator.Send(new UpdateOptionCommand(id, dto), ct);
            return HandleResult(response);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOption(int id, CancellationToken ct)
        {
            var response = await mediator.Send(new DeleteOptionCommand(id), ct);
            return HandleResult(response);
        }
    }
}
