using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Commands
{
    public class DeActivateProductHandler : IRequestHandler<DeActivateProductCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _uow;

        public DeActivateProductHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<Unit>> Handle(DeActivateProductCommand request, CancellationToken ct)
        {
            var entity = await _uow.ProductsI.GetToUpdateAsync(request.Id, ct);
            if (entity is null)
                return Result<Unit>.Failure(false, "Product not found", 404);
            if (!entity.IsActive)
                return Result<Unit>.Failure(false, "Product is already deactivated", 400);

            entity.IsActive = false;
            await _uow.SaveChangesAsync(ct);
            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}
