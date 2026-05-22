using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.OptionsDtos;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.OptionsCQ.Commands
{

    public record UpdateOptionCommand(int Id, UpdateOptionDto Dto) : IRequest<Result<OptionDto>>;

    public class UpdateOptionHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<UpdateOptionCommand, Result<OptionDto>>
    {
        public async Task<Result<OptionDto>> Handle(UpdateOptionCommand request, CancellationToken ct)
        {
            var entity = await uow.Options.GetToUpdateAsync(request.Id, ct);

            if (entity is null)
                return Result<OptionDto>.Failure(false, "Opción no encontrada", 404);

            mapper.Map(request.Dto, entity);
            await uow.SaveChangesAsync(ct);

            var dto = mapper.Map<OptionDto>(entity);
            return Result<OptionDto>.Success(dto, 200);
        }
    }
}
