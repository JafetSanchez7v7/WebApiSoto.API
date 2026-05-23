using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.CustomersCQ.Commands
{
    public record DeactivateCustomerCommand(int id): IRequest<Result<Unit>>;
    public class DeactivateCustomerHandler(IUnitOfWork context) : IRequestHandler<DeactivateCustomerCommand,Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(DeactivateCustomerCommand request, CancellationToken ct)
        {
            var customerToUpdate = await context.Customers.GetByIdAsync(request.id, ct);
            if (customerToUpdate is null)
                return Result<Unit>.Failure(false, "No se encontro al Cliente Solicitado", 404);
            // validamos que no este descativado
            if (!customerToUpdate.IsActive)
                return Result<Unit>.Failure(false, "El Cliente que quiere descativar ya esta Inactivo", 409);
            //desactivamos
            await context.Customers.DeactivateAsync(request.id, ct);
            // Devolvemos la respuesta

            return Result<Unit>.Success(Unit.Value, 204);
             
        }
            
    }
}
