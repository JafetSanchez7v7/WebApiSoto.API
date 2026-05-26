using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Sales;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.SalesCQ.Commands
{
    public record AddSaleCommand(CreateSaleDto dto) : IRequest<Result<SaleDto>>;
    public class AddSalesHandler(IMapper mapper, IUnitOfWork context) : IRequestHandler<AddSaleCommand, Result<SaleDto>>
    {
        public async Task<Result<SaleDto>> Handle(AddSaleCommand request, CancellationToken ct)
        {
            var existingCustomer = await context.Customers.GetByIdAsync(request.dto.CustomerId, ct);
            if (existingCustomer is null || !existingCustomer.IsActive)
                return Result<SaleDto>.Failure(false, "El cliente seleccionado no existe o esta inactivo", 400);

            var productsToSell = request.dto.SaleDetails.Select(x => x.ProductId).Distinct().ToList();
            var productsInCatalog = await context.ProductsI.GetWhereAsync(x => productsToSell.Contains(x.ProductId), ct);

            if (productsToSell.Count != productsInCatalog.Count())
            {
                var foundIds = productsInCatalog.Select(p => p.ProductId);
                var missingIds = productsToSell.Except(foundIds).ToList();
                return Result<SaleDto>.Failure(false, $"Algunos de los productos solicitados no se encuentran en catalogo por favor intente de nuevo, MissingIds: {string.Join(", ", missingIds)}", 400);
            }

            foreach (var product in productsInCatalog)
            {
                if (!product.IsActive)
                    return Result<SaleDto>.Failure(false, $"El producto con id: {product.ProductId} esta inactivo", 400);
            }

            try
            {
                await context.BeginTransactionAsync(ct);
                var sale = mapper.Map<Sale>(request.dto);
                var inventories = await context.Inventory.GetWhereAsync(i => productsToSell.Contains(i.ProductId), ct);
                var stockMap = inventories.ToDictionary(i => i.ProductId);

                foreach (var detail in sale.SaleDetails)
                {
                    if (detail.ProductId is null || !stockMap.TryGetValue(detail.ProductId.Value, out var inventory) || detail.Quantity > inventory.Quantity)
                    {
                        await context.RollbackTransactionAsync(ct);
                        return Result<SaleDto>.Failure(false, "Stock insuficiente", 400);
                    }

                    inventory.Quantity -= detail.Quantity;
                    detail.LineAmount = detail.Quantity * inventory.SalePrice;
                }

                sale.SaleTotal = sale.SaleDetails.Sum(x => x.LineAmount);
                sale.SaleDate = DateTime.Now;
                var addedSale = await context.Sales.AddSaleAsync(sale, ct);
                await context.SaveChangesAsync(ct);
                var newSale = await context.Sales.GetByIdAsync(addedSale.SaleId, ct);
                await context.CommitTransactionAsync(ct);

                var mapped = mapper.Map<SaleDto>(newSale);
                return Result<SaleDto>.Success(mapped, 201);
            }
            catch (Exception)
            {
                await context.RollbackTransactionAsync(ct);
                return Result<SaleDto>.Failure(false, "ocurrio un error al registrar la venta por favor intente mas tarde", 500);
            }
        }
    }
}
