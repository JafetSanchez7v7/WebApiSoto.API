using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Customers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.CustomersCQ.Commands
{
    public record UpdateCustomerCommand(int id,UpdateCustomerDto dto) : IRequest<Result<Unit>>;
    public  class UpdateCustomerHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<UpdateCustomerCommand ,Result<Unit>> 
    {
        public async Task<Result<Unit>> Handle(UpdateCustomerCommand request, CancellationToken ct)
        {
            //Validamos que el cliente sea existente
            var customerToUpdate = await context.Customers.GetToUpdateAsync(request.id, ct);
            if (customerToUpdate is null)
                return Result<Unit>.Failure(false,"Cliente Proporcionado no existe", 204);

            //Validacion de nombres y propiedad de nombre si el id es dueño del nombre no pasa nada
            var isConflictedName = await context.Customers.GetByNameAsync(request.dto.Name, ct);
            if (isConflictedName != null && isConflictedName.CustomerId != request.id)
                return Result<Unit>.Failure(false, "El Nombre proporcionado ya esta en uso", 409);

            //ChangeTracking
            mapper.Map(request.dto, customerToUpdate);

            await context.Customers.UpdateAsync(ct);

            return Result<Unit>.Success(Unit.Value, 204);
        }
    }
}
