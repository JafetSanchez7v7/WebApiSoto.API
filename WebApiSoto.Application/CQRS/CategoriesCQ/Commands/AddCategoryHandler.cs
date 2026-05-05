using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Categories;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.CategoriesCQ.Commands
{
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AddCategoryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(AddCategoryCommand request, CancellationToken ct)
        {
            var exists = await _uow.Category.GetByNameAsync(request.Dto.CategoryName, ct);
            if (exists is not null)
                return Result<CategoryDto>.Failure(true, "Category already exists", 409);

            var entity = _mapper.Map<Categories>(request.Dto);
            await _uow.Category.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            var dto = _mapper.Map<CategoryDto>(entity);
            return Result<CategoryDto>.Success(dto, 201);
        }
    }
}