using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.OptionsCQ.Commands
{
    public record DeleteOptionCommand(int Id) : IRequest<Result<object>>;

    public class DeleteOptionHandler(IUnitOfWork uow)
        : IRequestHandler<DeleteOptionCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteOptionCommand request, CancellationToken ct)
        {
            var exists = await uow.Options.GetOptionById(request.Id, ct);

            if (exists is null)
                return Result<object>.Failure(false, "Opción no encontrada", 404);

            await uow.Options.DeleteOption(request.Id, ct);
            await uow.SaveChangesAsync(ct);

            return Result<object>.Success(null!, 204);
        }
    }
}
