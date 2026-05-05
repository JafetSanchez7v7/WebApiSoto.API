using MediatR;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Commands
{
    public record DeActivateProductCommand(int Id) : IRequest<Result<Unit>>;
}
