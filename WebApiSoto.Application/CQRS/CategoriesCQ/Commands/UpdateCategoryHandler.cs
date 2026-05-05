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
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var entity = await _uow.Category.GetToUpdateAsync(request.Id, ct);
            if (entity is null)
                return Result<CategoryDto>.Failure(true, "Category not found", 404);

            _mapper.Map(request.Dto, entity);
            await _uow.SaveChangesAsync(ct);

            var dto = _mapper.Map<CategoryDto>(entity);
            return Result<CategoryDto>.Success(dto, 200);
        }
    }
}