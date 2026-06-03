using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.OrdersCQ
{
    public record UpdateOrderStatusCommand(UpdateOrderStatusDto Dto) : IRequest<Result<OrderDto>>;

    public class UpdateOrderStatusHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<UpdateOrderStatusCommand, Result<OrderDto>>
    {
        private const int PersonalizedProductThreshold = 50001;

        public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            // 1. Validar que el pedido exista
            var order = await uow.Orders.GetToUpdateAsync(dto.OrderId, ct);
            if (order is null)
                return Result<OrderDto>.Failure(false, "No existe un pedido con ese Id", 404);

            // 2. Obtener detalles del pedido
            var orderWithDetails = await uow.Orders.GetByIdAsync(dto.OrderId, ct);
            var normalDetails = orderWithDetails!.OrderDetails
                .Where(x => x.ProductId < PersonalizedProductThreshold)
                .ToList();

            try
            {
                await uow.BeginTransactionAsync(ct);

                // 3. Actualizar estado
                order.IsActive = dto.IsActive;

                // ── ESTADO 3: ABORTADO → devolver stock reservado ──────────
                if (dto.IsActive == 3)
                {
                    foreach (var detail in normalDetails)
                    {
                        var inventory = await uow.Inventory.GetInventoryByProductId(detail.ProductId, ct);
                        if (inventory is not null)
                        {
                            var inventoryTracked = await uow.Inventory.GetToUpdateAsync(inventory.InventoryId, ct);
                            if (inventoryTracked is not null)
                            {
                                inventoryTracked.Quantity = (inventoryTracked.Quantity ?? 0) + detail.Quantity;
                                inventoryTracked.ReservedStock = (inventoryTracked.ReservedStock ?? 0) - detail.Quantity;
                            }
                        }
                    }
                } 

                // ── ESTADO 2: VENDIDO → consumir reservado y crear venta ───
                if (dto.IsActive == 2)
                {
                    var saleDetails = new List<SaleDetail>();

                    foreach (var detail in normalDetails)
                    {
                        var inventory = await uow.Inventory.GetInventoryByProductId(detail.ProductId, ct);
                        if (inventory is not null)
                        {
                            // Restar stock reservado
                            var inventoryTracked = await uow.Inventory.GetToUpdateAsync(inventory.InventoryId, ct);
                            if (inventoryTracked is not null)
                            {
                                inventoryTracked.ReservedStock =
                                                 (inventoryTracked.ReservedStock ?? 0) - detail.Quantity;
                            }

                            // Construir detalle de venta
                            saleDetails.Add(new SaleDetail
                            {
                                ProductId = detail.ProductId,
                                Quantity = detail.Quantity,
                                LineAmount = inventory.SalePrice * detail.Quantity
                            });
                        }
                    }


                    // Crear la venta
                    var sale = new Sale
                    {
                        CustomerId = orderWithDetails.CustomerId,
                        SaleDate = DateTime.UtcNow,
                        SaleTotal = orderWithDetails.TotalAmount,
                        SaleDetails = saleDetails
                    };

                    var factura = mapper.Map<Invoice>(sale);
                    sale.Invoice = factura;

                    await uow.Sales.AddSaleAsync(sale, ct);
                }

                
                await uow.SaveChangesAsync(ct);
                
                await uow.CommitTransactionAsync(ct);
                var newOrder = await uow.Orders.GetByIdAsync(order.OrderId, ct);

                return Result<OrderDto>.Success(mapper.Map<OrderDto>(newOrder), 200);
            }
            catch (Exception)
            {
                await uow.RollbackTransactionAsync(ct);
                return Result<OrderDto>.Failure(false, "Ocurrió un error al actualizar el estado del pedido", 500);
            }
        }
    }
}