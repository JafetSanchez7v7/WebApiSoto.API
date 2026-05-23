using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.Common.DTOs.OptionsDtos;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.OptionsCQ.Queries
{
    public record GetOptionsQuery(FIlterOptionsDto dto): IRequest<Result<PaginationList<OptionDto>>>;
    public class GetOptionHandler(IUnitOfWork context, IMapper mapper ) : IRequestHandler<GetOptionsQuery, Result<PaginationList<OptionDto>>>
    {
        public async Task<Result<PaginationList<OptionDto>>> Handle(GetOptionsQuery request, CancellationToken ct)
        {
            var response = await context.Options.GetOptions(request.dto, ct);
            if (!response.Any())
                return Result<PaginationList<OptionDto>>.Failure(true, "No Registers", 200);

            var totalPages = (int)Math.Ceiling((double)response.Count() / request.dto.PageSize);
            var mapped = mapper.Map<List<OptionDto>>(response);
            var pagination = new PaginationList<OptionDto>(mapped, request.dto.PageNumber, totalPages);

            return Result<PaginationList<OptionDto>>.Success(pagination, 200);

        }
    
    }
}
