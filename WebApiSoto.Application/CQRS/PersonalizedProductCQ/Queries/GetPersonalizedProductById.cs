using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.PersonalizedProduct;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.PersonalizedProductCQ.Queries
{
    public class GetPersonalizedProductById
    {

        public record GetPersonalizedProductByIdQuery(int Id) : IRequest<Result<PersonalizedProductDto>>;

        public class GetPersonalizedProductByIdHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<GetPersonalizedProductByIdQuery, Result<PersonalizedProductDto>>
        {
            public async Task<Result<PersonalizedProductDto>> Handle(GetPersonalizedProductByIdQuery request, CancellationToken ct)
            {
                var product = await uow.PersonalizedProducts.GetByIdAsync(request.Id, ct);

                if (product is null)
                    return Result<PersonalizedProductDto>.Failure(false, "Producto personalizado no encontrado", 404);

                return Result<PersonalizedProductDto>.Success(mapper.Map<PersonalizedProductDto>(product), 200);
            }
        }
    }
}
