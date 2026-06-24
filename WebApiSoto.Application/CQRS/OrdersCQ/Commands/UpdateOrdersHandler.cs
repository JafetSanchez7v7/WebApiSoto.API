using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Order;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.OrdersCQ.Commands
{

    public class UpdateOrderHandler
    {
        public record UpdateOrderCommand(UpdateOrderDto Dto) : IRequest<Result<OrderDto>>;

        public class UpdateOrdersHandler(IUnitOfWork uow, IMapper mapper)
            : IRequestHandler<UpdateOrderCommand, Result<OrderDto>>
        {
            public async Task<Result<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken ct)
            {
                var dto = request.Dto;

                // 1. Verificar que el pedido existe
                var existing = await uow.Orders.GetToUpdateAsync(dto.OrderId, ct);
                if (existing is null)
                    return Result<OrderDto>.Failure(false, "El pedido no existe", 404);

                // 2. Solo se puede editar si está Pendiente
                if (existing.IsActive != 1)
                    return Result<OrderDto>.Failure(false, "Solo se pueden editar pedidos en estado Pendiente", 400);

                // 3. Verificar cliente
                var customer = await uow.Customers.GetByIdAsync(dto.CustomerId, ct);
                if (customer is null || !customer.IsActive)
                    return Result<OrderDto>.Failure(false, "El cliente no existe o está inactivo", 400);

                try
                {
                    await uow.BeginTransactionAsync(ct);

                    var newDetails = new List<OrderDetail>();

                    foreach (var detail in dto.OrderDetails)
                    {
                        var existingDetail = existing.OrderDetails
                            .FirstOrDefault(x => x.ProductId == detail.ProductId);

                        var inventory = await uow.Inventory.GetInventoryByProductId(detail.ProductId, ct);
                        if (inventory is null)
                            return Result<OrderDto>.Failure(false, $"El producto con Id {detail.ProductId} no existe en el inventario", 404);

                        decimal salePrice = inventory.SalePrice ?? 0;
                        var volume = detail.Volume > 0 ? detail.Volume : 1;

                        if (existingDetail is not null)
                        {
                            var totalQuantity = existingDetail.Quantity + detail.Quantity;
                            newDetails.Add(new OrderDetail
                            {
                                OrderId = dto.OrderId,
                                ProductId = detail.ProductId,
                                Quantity = totalQuantity,
                                Volume = detail.Volume,
                                SalePrice = salePrice,
                                Total = salePrice * totalQuantity * volume
                            });
                        }
                        else
                        {
                            newDetails.Add(new OrderDetail
                            {
                                OrderId = dto.OrderId,
                                ProductId = detail.ProductId,
                                Quantity = detail.Quantity,
                                Volume = detail.Volume,
                                SalePrice = salePrice,
                                Total = salePrice * detail.Quantity * volume
                            });
                        }
                    }

                    var totalAmount = newDetails.Sum(x => x.Total);

                    var updated = new Order
                    {
                        OrderId = existing.OrderId,
                        CustomerId = dto.CustomerId,
                        OrderDate = existing.OrderDate,
                        TimeDelivery = dto.TimeDelivery,
                        TotalAmount = totalAmount,
                        HalfPayment = totalAmount / 2,
                        IsActive = existing.IsActive,
                        OrderDetails = newDetails
                    };

                    await uow.Orders.UpdateAsync(updated, ct);
                    await uow.SaveChangesAsync(ct);
                    await uow.CommitTransactionAsync(ct);

                    var result = await uow.Orders.GetByIdAsync(dto.OrderId, ct);
                    return Result<OrderDto>.Success(mapper.Map<OrderDto>(result), 200);
                }
                catch (Exception)
                {
                    await uow.RollbackTransactionAsync(ct);
                    return Result<OrderDto>.Failure(false, "Ocurrió un error al actualizar el pedido", 500);
                }
            }
        }
    }
}
