using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.CategoriesCQ.Commands;
using WebApiSoto.Application.CQRS.CategoriesCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController: ApiController
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Gerent,Operator")]
        public async Task<IActionResult> GetCategories([FromQuery] FiltersDto dto, IValidator<FiltersDto> validator, CancellationToken ct)
        {
            var validation = validator.Validate(dto);
            if (validation.Errors.Any())
            {
                var errors = validation.Errors.Select(c => c.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            var response = await _mediator.Send(new GetCategoriesQuery(dto), ct);
            return HandleResult(response);
        }
        [Authorize(Roles = "Admin,Gerent,Operator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetCategoryByIdQuery(id), ct);
            return HandleResult(response);
        }
        [Authorize(Roles = "Admin,Gerent")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto, CancellationToken ct,[FromServices] IValidator<CreateCategoryDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new AddCategoryCommand(dto), ct);
            if (!response.IsSuccess)
                return HandleResult(response);

            return CreatedAtAction(nameof(GetById), new { id = response.Value.CategoryId }, response);
        }
        [Authorize(Roles = "Admin,Gerent")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto, CancellationToken ct,[FromServices] IValidator<UpdateCategoryDto> validator)
        {
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(new UpdateCategoryCommand(id, dto), ct);
            return HandleResult(response);
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin,Gerent")]
        public async Task<IActionResult> DeactivateCategory(int id, CancellationToken ct,[FromServices] IValidator<DeactivateCategoryCommand> validator)
        {
            var command = new DeactivateCategoryCommand(id);
            var validation = validator.Validate(command);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var response = await _mediator.Send(command, ct);
            return HandleResult(response);
        }
    }
}

