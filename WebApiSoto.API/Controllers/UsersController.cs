using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.DTOs.Users;
using WebApiSoto.Application.Common.DTOs.Validators.UsersValidators;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.UserDto.Commands;
using WebApiSoto.Application.CQRS.UserDto.Queries;
using WebApiSoto.Application.CQRS.Users.Commands;
using WebApiSoto.Application.CQRS.UsersCQ.Commands;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiController
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator media)
        {
            _mediator = media;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] FiltersDto dto, CancellationToken ct, IValidator<FiltersDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(m => m.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var result = await _mediator.Send(new GetUserQuery(dto), ct);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id), ct);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto dto, CancellationToken ct, IValidator<CreateUserDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(m => m.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var result = await _mediator.Send(new AddUserCommand(dto), ct);
            if (!result.IsSuccess)
                return HandleResult(result);

            return CreatedAtAction(nameof(GetUserById), new { Id = result.Value.Id }, result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto, CancellationToken ct, IValidator<UpdateUserDto> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(m => m.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            var result = await _mediator.Send(new UpdateUserCommand(id, dto), ct);
            return HandleResult(result);

        }

        [HttpPatch("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDto dto, CancellationToken ct,[FromServices] IValidator<UpdatePasswordDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(m => m.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            var result = await _mediator.Send(new UpdatePasswordCommand(id, dto), ct);
            return HandleResult(result);

        }

        [HttpPatch("{id}/deactivate") ]
        public async Task<IActionResult> DeActivateUser(int id, [FromServices] IValidator<DeActivateUserCommand> validator, CancellationToken ct)
        {
            if(id < 0)
            {
                return BadRequest("proporcione un id valido");
            }

            var response = await _mediator.Send(new DeActivateUserCommand(id),ct );

            return HandleResult(response);
        }
    }
}
