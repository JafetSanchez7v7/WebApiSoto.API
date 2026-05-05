using MediatR;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.Application.CQRS.CategoriesCQ.Commands
{
    public record UpdateCategoryCommand(int Id, UpdateCategoryDto Dto) : IRequest<Result<WebApiSoto.Application.Common.DTOs.Categories.CategoryDto>>;
}