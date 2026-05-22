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
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.OptionsCQ.Commands
{
    public record CreateOptionCommand(CreateOptionDto Dto) : IRequest<Result<OptionDto>>;

    public class CreateOptionHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<CreateOptionCommand, Result<OptionDto>>
    {
        public async Task<Result<OptionDto>> Handle(CreateOptionCommand request, CancellationToken ct)
        {
            var option = mapper.Map<Option>(request.Dto);
            var created = await uow.Options.CreateOption(option, ct);
            await uow.SaveChangesAsync(ct);

            var mapped = mapper.Map<OptionDto>(created);
            return Result<OptionDto>.Success(mapped, 201);
        }
    }
}
