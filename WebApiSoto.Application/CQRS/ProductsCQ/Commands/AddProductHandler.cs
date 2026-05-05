using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Products;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Commands
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, Result<ProductDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AddProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(AddProductCommand request, CancellationToken ct)
        {
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
            if (duplicate is not null)
                return Result<ProductDto>.Failure(true, "Product already exists", 409);

            var entity = _mapper.Map<Products>(request.Dto);
            await _uow.ProductsI.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            var saved = await _uow.ProductsI.GetToUpdateAsync(entity.ProductId, ct);
            if (saved is null)
                return Result<ProductDto>.Failure(false, "Product could not be loaded", 500);

            var dto = _mapper.Map<ProductDto>(saved);
            return Result<ProductDto>.Success(dto, 201);
        }
    }
}
