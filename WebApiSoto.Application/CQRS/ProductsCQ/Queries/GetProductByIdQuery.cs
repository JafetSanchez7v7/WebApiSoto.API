using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Products;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<Result<ProductDto>>;

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
        {
            var entity = await _uow.ProductsI.GetByIdAsync(request.Id, ct);
            if (entity is null)
                return Result<ProductDto>.Failure(false, "Product not found", 404);

            var dto = _mapper.Map<ProductDto>(entity);
            return Result<ProductDto>.Success(dto, 200);
        }
    }
}
