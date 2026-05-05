using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Products;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.ProductsCQ.Queries
{
    public record GetAllProductsQuery(FiltersDto dto) : IRequest<Result<PaginationList<ProductDto>>>;

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<PaginationList<ProductDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<PaginationList<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken ct)
        {
            var response = await _uow.ProductsI.GetAllAsync(request.dto, ct);
            
            if (!response.Any())
                return Result<PaginationList<ProductDto>>.Failure(true, "No Registers", 200);

            var totalPages = (int)Math.Ceiling((double)response.Count() / request.dto.PageSize);

            var mapped = _mapper.Map<List<ProductDto>>(response);
           
            
            var pagination = new PaginationList<ProductDto>(mapped, request.dto.PageNumber, totalPages);
            return Result<PaginationList<ProductDto>>.Success(pagination, 200);
        }

       
    }
}
