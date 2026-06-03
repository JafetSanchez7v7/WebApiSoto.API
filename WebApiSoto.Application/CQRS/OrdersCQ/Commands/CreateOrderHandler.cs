using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.OrdersCQ.Commands
{
    public class CreateOrderHandler
    {
        public record CreateOrderCommand(CreateOrderDto Dto) : IRequest<Result<OrderDto>>;

        public class CreateOrdersHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
        {
            private const int PersonalizedProductThreshold = 50000;

            public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken ct)
            {
                var dto = request.Dto;

                // 1. Validar que el cliente exista y esté activo
                var customer = await uow.Customers.GetByIdAsync(dto.CustomerId, ct);
                if (customer is null || !customer.IsActive)
                    return Result<OrderDto>.Failure(false, "El cliente seleccionado no existe o está inactivo", 400);

                try
                {
                    await uow.BeginTransactionAsync(ct);

                    var orderDetails = new List<OrderDetail>();

                  
                    foreach (var detail in dto.OrderDetails)
                    {
                        decimal salePrice = 0;

                        if (detail.ProductId < PersonalizedProductThreshold)
                        {
                            // Producto normal — obtener precio del inventario
                            var inventory = await uow.Inventory.GetInventoryByProductId(detail.ProductId, ct);
                            if (inventory is null)
                                return Result<OrderDto>.Failure(false, $"El producto con Id {detail.ProductId} no existe en el inventario", 404);

                            if (inventory.Quantity < detail.Quantity)
                                return Result<OrderDto>.Failure(false, $"No hay suficientes unidades para el producto con Id {detail.ProductId}", 400);

                            salePrice = inventory.SalePrice ?? 0;

                            // Actualizar inventario: restar quantity, sumar reservedStock
                            var inventoryTracked = await uow.Inventory.GetToUpdateAsync(inventory.InventoryId, ct);
                            inventoryTracked!.Quantity -= detail.Quantity;
                            inventoryTracked!.ReservedStock = (inventoryTracked.ReservedStock ?? 0) + detail.Quantity;
                        }
                        else
                        {
                            // Producto personalizado — obtener precio de PersonalizedProducts
                            var personalizedProduct = await uow.PersonalizedProducts.GetByIdAsync(detail.ProductId, ct);
                            if (personalizedProduct is null)
                                return Result<OrderDto>.Failure(false, $"El producto personalizado con Id {detail.ProductId} no existe", 404);

                            salePrice = personalizedProduct.SalePrice ?? 0;
                        }

                        var volume = detail.Volume > 0 ? detail.Volume : 1;

                        orderDetails.Add(new OrderDetail
                        {
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            Volume = detail.Volume,
                            SalePrice = salePrice,
                            Total = salePrice * detail.Quantity * volume
                        });
                    }

                    // 3. Calcular totales
                    var totalAmount = orderDetails.Sum(x => x.Total);

                    // 4. Construir el pedido
                    var order = new Order
                    {
                        CustomerId = dto.CustomerId,
                        OrderDate = DateTime.UtcNow,
                        TimeDelivery = dto.TimeDelivery,
                        TotalAmount = totalAmount,
                        HalfPayment = totalAmount / 2, 
                        IsActive = 1,
                        OrderDetails = orderDetails
                    };

                    await uow.Orders.AddAsync(order, ct);
                    await uow.SaveChangesAsync(ct);

                    // 5. Recargar con navegaciones para el mapeo
                    var created = await uow.Orders.GetByIdAsync(order.OrderId, ct);

                    await uow.CommitTransactionAsync(ct);

                    return Result<OrderDto>.Success(mapper.Map<OrderDto>(created), 201);
                }
                catch (Exception)
                {
                    await uow.RollbackTransactionAsync(ct);
                    return Result<OrderDto>.Failure(false, "Ocurrió un error al registrar el pedido, intente más tarde", 500);
                }
            }
        }
    }
}
