using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.Inventory;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.InventoryCQ.Queries
{
    
    public record GetInventoryQuery(FilterInventoryDto Dto) : IRequest<Result<PaginationList<InventoryDto>>>;

    public class GetInventoryHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<GetInventoryQuery, Result<PaginationList<InventoryDto>>>
    {
        public async Task<Result<PaginationList<InventoryDto>>> Handle(GetInventoryQuery request, CancellationToken ct)
        {
            var response = await uow.Inventory.GetInventory(request.Dto, ct);

            if (!response.Any())
                return Result<PaginationList<InventoryDto>>.Failure(true, "No Registers", 200);

            var totalPages = (int)Math.Ceiling((double)response.Count() / request.Dto.PageSize);
            var mapped = mapper.Map<List<InventoryDto>>(response);
            var pagination = new PaginationList<InventoryDto>(mapped, request.Dto.PageNumber, totalPages);

            return Result<PaginationList<InventoryDto>>.Success(pagination, 200);
        }
    }

    public record GetInventoryByIdQuery(int Id) : IRequest<Result<InventoryDto>>;

    public class GetInventoryByIdHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<GetInventoryByIdQuery, Result<InventoryDto>>
    {
        public async Task<Result<InventoryDto>> Handle(GetInventoryByIdQuery request, CancellationToken ct)
        {
            var inventory = await uow.Inventory.GetInventoryById(request.Id, ct);

            if (inventory is null)
                return Result<InventoryDto>.Failure(false, "Inventory not found", 404);

            return Result<InventoryDto>.Success(mapper.Map<InventoryDto>(inventory), 200);
        }
    }

   
    public record GetInventoryByProductIdQuery(int ProductId) : IRequest<Result<InventoryDto>>;

    public class GetInventoryByProductIdHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<GetInventoryByProductIdQuery, Result<InventoryDto>>
    {
        public async Task<Result<InventoryDto>> Handle(GetInventoryByProductIdQuery request, CancellationToken ct)
        {
            var inventory = await uow.Inventory.GetInventoryByProductId(request.ProductId, ct);

            if (inventory is null)
                return Result<InventoryDto>.Failure(false, "No inventory was found for that product", 404);

            return Result<InventoryDto>.Success(mapper.Map<InventoryDto>(inventory), 200);
        }
    }

 
    public record GetInventoryByProductNameQuery(string ProductName) : IRequest<Result<List<InventoryDto>>>;

    public class GetInventoryByProductNameHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<GetInventoryByProductNameQuery, Result<List<InventoryDto>>>
    {
        public async Task<Result<List<InventoryDto>>> Handle(GetInventoryByProductNameQuery request, CancellationToken ct)
        {
            var inventories = await uow.Inventory.GetInventoryByProductName(request.ProductName, ct);

            if (!inventories.Any())
                return Result<List<InventoryDto>>.Failure(false, "No se encontraron registros con ese nombre de producto", 404);

            return Result<List<InventoryDto>>.Success(mapper.Map<List<InventoryDto>>(inventories), 200);
        }
    }
}
