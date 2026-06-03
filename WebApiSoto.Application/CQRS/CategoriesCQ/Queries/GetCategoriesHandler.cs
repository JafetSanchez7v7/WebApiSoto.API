using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.CategoriesCQ.Queries
{
    public record GetCategoriesQuery(FiltersDto dto) : IRequest<Result<PaginationList<CategoryDto>>>;
    public class GetCategoriesHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<GetCategoriesQuery,Result<PaginationList<CategoryDto>>>{
    
        public async Task<Result<PaginationList<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
        {
            var response = await context.Category.GetCategoriesAsync(request.dto, ct);
            if (!response.Any())
                return Result<PaginationList<CategoryDto>>.Failure(true, "No Registers", 200);

            var totalCount = await context.Category.CountAsync(request.dto, ct);
            var totalPages = (int)Math.Ceiling((double)totalCount / request.dto.PageSize);
            var mapped = mapper.Map<List<CategoryDto>>(response);
            var pagination = new PaginationList<CategoryDto>(mapped, request.dto.PageNumber, totalPages, totalCount);
            return Result<PaginationList<CategoryDto>>.Success(pagination, 200);
               
            
        }
    }
}