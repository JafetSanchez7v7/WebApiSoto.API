using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
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
            var exitantCategory = await _uow.Category.GetByIdAsync(request.Id, ct);
            if (exitantCategory is null)
                return Result<Unit>.Failure(false, "Category not found", 404);
            if(!exitantCategory.IsActive)
                return Result<Unit>.Failure(false, "Category is already deactivated", 400);
            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}