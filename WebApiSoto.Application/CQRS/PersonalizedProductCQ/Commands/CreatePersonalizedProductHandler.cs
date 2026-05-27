using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.PersonalizedProduct;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.PersonalizedProductCQ.Commands
{
    public record CreatePersonalizedProductCommand(CreatePersonalizedProductDto Dto) : IRequest<Result<PersonalizedProductDto>>;

    public class CreatePersonalizedProductHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<CreatePersonalizedProductCommand, Result<PersonalizedProductDto>>
    {
        public async Task<Result<PersonalizedProductDto>> Handle(CreatePersonalizedProductCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            // 1. Validar que el cliente exista y esté activo
            var customer = await uow.Customers.GetByIdAsync(dto.CustomerId, ct);
            if (customer is null || !customer.IsActive)
                return Result<PersonalizedProductDto>.Failure(false, "El cliente seleccionado no existe o está inactivo", 400);

            // 2. Validar que el producto exista en inventario y tenga stock
            var inventory = await uow.Inventory.GetInventoryByProductId(dto.ProductId, ct);
            if (inventory is null)
                return Result<PersonalizedProductDto>.Failure(false, $"El producto con Id {dto.ProductId} no existe en el inventario", 404);

            if (inventory.Quantity < 1)
                return Result<PersonalizedProductDto>.Failure(false, $"No hay unidades disponibles en inventario para el producto {dto.ProductId}", 400);

            // 3. Validar que todas las opciones existan
            var optionIds = dto.PersonalizationDetails.Select(x => x.OptionId).Distinct().ToList();
            var optionsInCatalog = await uow.Options.GetWhereAsync(x => optionIds.Contains(x.OptionId), ct);

            if (optionIds.Count != optionsInCatalog.Count())
            {
                var foundIds = optionsInCatalog.Select(o => o.OptionId);
                var missingIds = optionIds.Except(foundIds).ToList();
                return Result<PersonalizedProductDto>.Failure(false, $"Las siguientes opciones no existen: {string.Join(", ", missingIds)}", 400);
            }

            try
            {
                await uow.BeginTransactionAsync(ct);

                // 4. Calcular detalles de personalización y total de opciones
                var optionsMap = optionsInCatalog.ToDictionary(o => o.OptionId);
                decimal optionsTotal = 0;
                var personalizationDetails = new List<Personalization>();

                foreach (var detail in dto.PersonalizationDetails)
                {
                    var option = optionsMap[detail.OptionId];
                    var subTotal = detail.Quantity * (option.Price ?? 0);
                    optionsTotal += subTotal;

                    personalizationDetails.Add(new Personalization
                    {
                        OptionId = detail.OptionId,
                        Quantity = detail.Quantity,
                        SalePrice = option.Price,
                        SubTotal = subTotal
                    });
                }

                // 5. Calcular precio final = precio base del inventario + total opciones
                var finalPrice = (inventory.SalePrice ?? 0) + optionsTotal;

                // 6. Construir el producto personalizado
                var personalizedProduct = new PersonalizedProduct
                {
                    CustomerId = dto.CustomerId,
                    ProductId = dto.ProductId,
                    Description = dto.Description,
                    CreationDate = DateTime.UtcNow,
                    SalePrice = finalPrice,
                    Personalizations = personalizationDetails
                };

                // 7. Insertar producto personalizado
                await uow.PersonalizedProducts.AddAsync(personalizedProduct, ct);

                // 8. Restar 1 unidad del inventario
                var inventoryTracked = await uow.Inventory.GetToUpdateAsync(inventory.InventoryId, ct);
                inventoryTracked!.Quantity -= 1;

                await uow.SaveChangesAsync(ct);

                // 9. Recargar con navegaciones para el mapeo
                var created = await uow.PersonalizedProducts.GetByIdAsync(personalizedProduct.PersonalizedId, ct);

                await uow.CommitTransactionAsync(ct);

                var mapped = mapper.Map<PersonalizedProductDto>(created);
                return Result<PersonalizedProductDto>.Success(mapped, 201);
            }
            catch (Exception)
            {
                await uow.RollbackTransactionAsync(ct);
                return Result<PersonalizedProductDto>.Failure(false, "Ocurrió un error al registrar el producto personalizado, intente más tarde", 500);
            }
        }
    }
}