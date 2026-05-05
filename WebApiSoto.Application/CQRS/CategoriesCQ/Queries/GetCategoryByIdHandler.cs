using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.CategoriesCQ.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDto>>;
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper mapper;

        public GetCategoryByIdHandler(IUnitOfWork uow, IMapper map)
        {
            _uow = uow;
            mapper = map;
        }

        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken ct)
        {
            var category = await _uow.Category.GetByIdAsync(request.Id, ct);
            if (category is null)
                return Result<CategoryDto>.Failure(false, "Category not found", 404);
            var mapped = mapper.Map<CategoryDto>(category);


            return Result<CategoryDto>.Success(mapped, 200);
        }
    }
}