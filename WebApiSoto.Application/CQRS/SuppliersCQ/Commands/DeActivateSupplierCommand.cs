using MediatR;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.SuppliersCQ.Commands
{
    public record DeActivateSupplierCommand(int Id) : IRequest<Result<Unit>>;

    public class DeActivateSupplierHandler : IRequestHandler<DeActivateSupplierCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _uow;

        public DeActivateSupplierHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<Unit>> Handle(DeActivateSupplierCommand request, CancellationToken ct)
        {
            var existingSupplier = await _uow.Supplier.GetToUpdateAsync(request.Id, ct);
            if (existingSupplier is null)
                return Result<Unit>.Failure(false, "Supplier not found", 404);
            if (!existingSupplier.IsActive)
                return Result<Unit>.Failure(false, "Supplier is already deactivated", 400);

            existingSupplier.IsActive = false;
            await _uow.SaveChangesAsync(ct);
            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}
