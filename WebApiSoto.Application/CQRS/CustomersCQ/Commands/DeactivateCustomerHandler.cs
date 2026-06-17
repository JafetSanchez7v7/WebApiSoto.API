using MediatR;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.CustomersCQ.Commands
{
    public record DeactivateCustomerCommand(int id) : IRequest<Result<Unit>>;

    public class DeactivateCustomerHandler(IUnitOfWork context) : IRequestHandler<DeactivateCustomerCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(DeactivateCustomerCommand request, CancellationToken ct)
        {
            var customer = await context.Customers.GetToUpdateAsync(request.id, ct);

            if (customer is null)
                return Result<Unit>.Failure(false, "No se encontró al Cliente Solicitado", 404);

            customer.IsActive = !customer.IsActive;

            await context.Customers.UpdateAsync(ct);

            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}