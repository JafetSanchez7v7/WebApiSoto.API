using MediatR;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.Application.CQRS.CategoriesCQ.Commands
{
    public record DeactivateCategoryCommand(int Id) : IRequest<Result<Unit>>;
}