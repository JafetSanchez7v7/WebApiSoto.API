using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.PersonalizedProduct;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.PersonalizedProductCQ.Queries
{
    public class GetAllPersonalizedProduct
    {
        public record GetAllPersonalizedProductsQuery(FiltersDto Dto)
            : IRequest<Result<PaginationList<PersonalizedProductDto>>>;

        public class GetAllPersonalizedProductsHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<GetAllPersonalizedProductsQuery, Result<PaginationList<PersonalizedProductDto>>>
        {
            public async Task<Result<PaginationList<PersonalizedProductDto>>> Handle(
                GetAllPersonalizedProductsQuery request, CancellationToken ct)
            {
                var products = await uow.PersonalizedProducts.GetAllAsync(request.Dto, ct);

                if (!products.Any())
                    return Result<PaginationList<PersonalizedProductDto>>
                        .Failure(true, "No Registers", 200);

                var totalCount = await uow.PersonalizedProducts.CountAsync(request.Dto, ct);
                var totalPages = (int)Math.Ceiling(totalCount / (double)request.Dto.PageSize);
                var mapped = mapper.Map<List<PersonalizedProductDto>>(products);
                var paginationList = new PaginationList<PersonalizedProductDto>(
                    mapped, request.Dto.PageNumber, totalPages, totalCount);

                return Result<PaginationList<PersonalizedProductDto>>.Success(paginationList, 200);
            }
        }
    }
}