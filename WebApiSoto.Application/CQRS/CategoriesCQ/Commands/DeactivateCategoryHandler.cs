using MediatR;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.CategoriesCQ.Commands
{
    public class DeactivateCategoryHandler : IRequestHandler<DeactivateCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _uow;

        public DeactivateCategoryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<Unit>> Handle(DeactivateCategoryCommand request, CancellationToken ct)
        {
            var category = await _uow.Category.GetToUpdateAsync(request.Id, ct);

            if (category is null)
                return Result<Unit>.Failure(false, "Category not found", 404);

            category.IsActive = !category.IsActive;

            await _uow.Category.UpdateAsync(ct);

            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}