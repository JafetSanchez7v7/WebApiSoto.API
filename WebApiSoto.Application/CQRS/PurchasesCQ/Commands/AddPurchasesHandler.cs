using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Purchases;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.PurchasesCQ.Commands
{
    public record AddPurchaseCommand(CreatePurchaseDto dto) : IRequest<Result<PurchaseDto>>;
    public class AddPurchasesHandler(IMapper mapper, IUnitOfWork context) : IRequestHandler<AddPurchaseCommand, Result<PurchaseDto>>
    {
        public async Task<Result<PurchaseDto>> Handle(AddPurchaseCommand request, CancellationToken ct)
        {
            //Validaciones
            var existingSupplier = await context.Supplier.GetByIdAsync(request.dto.SupplierId, ct);
            if (existingSupplier is null || !existingSupplier.IsActive)
                return Result<PurchaseDto>.Failure(false, "El proveedor seleccionado no existe o esta inactivo", 400);

            //Productos en detalles
            //lista de productos solicitados hacemos la comparacion en memoria
            var productsToBuy = request.dto.PurchaseDetails.Select(X => X.ProductId).Distinct().ToList();
            // validacion de existencia
            var productsInCatalog = await context.ProductsI.GetWhereAsync(x => productsToBuy.Contains(x.ProductId) , ct);

            if(productsToBuy.Count != productsInCatalog.Count())
            {
                var foundIds = productsInCatalog.Select(p => p.ProductId);
                var missingIds = productsToBuy.Except(foundIds).ToList();
                return Result<PurchaseDto>.Failure(false, $"Algunos de los productos solicitados no se encuentran en catalogo por favor registrelos e intente de nuevo, MissingIds: {string.Join(", ", missingIds)}", 400);
            }
            // Validamos estado y proveedores de productos
            foreach(var product in productsInCatalog)
            {
                if ( !product.IsActive)
                    return Result<PurchaseDto>.Failure(false, $"El producto con id: {product.ProductId} esta inactivo", 400);

                if(product.SupplierId != request.dto.SupplierId)
                    return Result<PurchaseDto>.Failure(false, $"el proveedor del producto con id: {product.ProductId} no coincide con el proveedor registrado, si cambiara de proveedor por favor actualize el catalogo", 400);
            }
            //PROCEDEMOS CON LA COMPRA
            try
            {
                await context.BeginTransactionAsync(ct);
                var purchase = mapper.Map<Purchase>(request.dto);
                //traemos los inventarios para no hacer muchas llamadas a bd
                var stocks = await context.Inventory.GetWhereAsync(i => productsToBuy.Contains(i.ProductId), ct);
                // mejora de rendimiento
                var stockMap = stocks.ToDictionary(i => i.ProductId);
                // actualizamos inventario o creamos
                foreach (var detail in purchase.PurchaseDetails)
                {
                    if(detail.Quantity < 0)
                    {
                        await context.RollbackTransactionAsync(ct);
                        return Result<PurchaseDto>.Failure(false,"No se permite stock negativo para las compras",400);
                    }
                    if (stockMap.TryGetValue(detail.ProductId, out var inventory))
                    {
                        inventory.Quantity += detail.Quantity;
                        inventory.PurchasePrice = detail.PurchasePrice;
                        inventory.SalePrice = detail.PurchasePrice * 1.10m;

                    }
                    else
                    {
                        inventory = new Inventory
                        {
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            PurchasePrice = detail.PurchasePrice,
                            SalePrice = detail.PurchasePrice * 1.10m,

                        };

                        await context.Inventory.AddAsync(inventory, ct);
                        stockMap.Add(inventory.ProductId, inventory);
                    }
                    detail.Total = detail.Quantity * detail.PurchasePrice;
                }
                purchase.TotalAmount = purchase.PurchaseDetails.Sum(x => x.Total);
                purchase.Date = DateTime.Now;
                var addedPurchase = await context.Purchases.AddPurchaseAsync(purchase, ct);
                // PERSISTIMOS EN DB Y CARGAMOS LAS PROPIEDADES DE NAVEGACION
                await context.SaveChangesAsync(ct);
                //traemos la compra recien registrada con props de navegacion
                var newPurchase = await context.Purchases.GetByIdAsync(addedPurchase.PurchaseId, ct);
                await context.CommitTransactionAsync(ct);
               
                //mapeo
                var mapped = mapper.Map<PurchaseDto>(newPurchase);
                return Result<PurchaseDto>.Success(mapped, 201);
            }
            catch (Exception )
            {
                await context.RollbackTransactionAsync(ct);
                return Result<PurchaseDto>.Failure(false, "ocurrio un error al registrar la compra por favor intente mas tarde", 500);
            }
           
        }

    }
}
