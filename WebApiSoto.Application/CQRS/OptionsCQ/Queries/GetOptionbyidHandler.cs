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

namespace WebApiSoto.Application.CQRS.OptionsCQ.Queries
{
    public class GetOptionbyidHandler
    {
        public record GetOptionByIdQuery(int Id) : IRequest<Result<OptionDto>>;

        public class GetOptionByIdHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<GetOptionByIdQuery, Result<OptionDto>>
        {
            public async Task<Result<OptionDto>> Handle(GetOptionByIdQuery request, CancellationToken ct)
            {
                var option = await uow.Options.GetOptionById(request.Id, ct);

                if (option is null)
                    return Result<OptionDto>.Failure(false, "Opción no encontrada", 404);

                var mapped = mapper.Map<OptionDto>(option);
                return Result<OptionDto>.Success(mapped, 200);
            }
        }
    }
}
