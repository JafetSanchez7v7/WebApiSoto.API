using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Products;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var entity = await _uow.ProductsI.GetToUpdateAsync(request.Id, ct);
            if (entity is null)
                return Result<ProductDto>.Failure(true, "Product not found", 404);

            var existingCategory = await _uow.Category.GetByIdAsync(request.Dto.CategoryId, ct);
            if (existingCategory is null)
                return Result<ProductDto>.Failure(false, "Category not found", 404);
            if (!existingCategory.IsActive)
                return Result<ProductDto>.Failure(false, "Category is inactive", 400);

            var existingSupplier = await _uow.Supplier.GetByIdAsync(request.Dto.SupplierId, ct);
            if (existingSupplier is null)
                return Result<ProductDto>.Failure(false, "Supplier not found", 404);
            if (!existingSupplier.IsActive)
                return Result<ProductDto>.Failure(false, "Supplier is inactive", 400);

            var duplicate = await _uow.ProductsI.GetByNameAsync(request.Dto.ProductName, ct);
            if (duplicate is not null && duplicate.ProductId != request.Id)
                return Result<ProductDto>.Failure(true, "Product already exists", 409);

            entity.ProductName = request.Dto.ProductName;
            entity.CategoryId = request.Dto.CategoryId;
            entity.SupplierId = request.Dto.SupplierId;
            entity.Description = request.Dto.Description;
            entity.IsActive = request.Dto.IsActive;

            await _uow.SaveChangesAsync(ct);

            var dto = _mapper.Map<ProductDto>(entity);
            return Result<ProductDto>.Success(dto, 200);
        }
    }
}
