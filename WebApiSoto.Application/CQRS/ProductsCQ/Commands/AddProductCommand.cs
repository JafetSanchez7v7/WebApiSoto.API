using MediatR;
using WebApiSoto.Application.Common.DTOs.Products;
using WebApiSoto.Application.Common.Models;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Commands
{
    public record AddProductCommand(CreateProductDto Dto) : IRequest<Result<ProductDto>>;
}
