using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.AuthService;
using WebApiSoto.Application.CQRS.UsersCQ.Commands;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController    {
        private readonly IMediator _mediator;
        private readonly IValidator<LoginRequestDto> validator;
        public AuthController(IValidator<LoginRequestDto> val, IMediator mediator)
        {
            (_mediator, validator) = (mediator, val);   
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var validation = validator.Validate(loginRequest);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(m => m.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            var response = await _mediator.Send(new LoginCommand(loginRequest));
            if(!response.IsSuccess)
                return HandleResult(response);
            else
            {
                return Ok(response);
            }
        }

       
    }
}
