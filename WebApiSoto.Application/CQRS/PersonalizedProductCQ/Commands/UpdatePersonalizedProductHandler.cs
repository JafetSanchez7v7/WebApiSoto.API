using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.PersonalizedProduct;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.PersonalizedProductCQ.Commands
{
    public record UpdatePersonalizedProductCommand(int Id, UpdatePersonalizedProductDto Dto)
        : IRequest<Result<PersonalizedProductDto>>;

    public class UpdatePersonalizedProductHandler(IUnitOfWork uow, IMapper mapper)
        : IRequestHandler<UpdatePersonalizedProductCommand, Result<PersonalizedProductDto>>
    {
        public async Task<Result<PersonalizedProductDto>> Handle(
            UpdatePersonalizedProductCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            var product = await uow.PersonalizedProducts.GetToUpdateAsync(request.Id, ct);
            if (product is null)
                return Result<PersonalizedProductDto>.Failure(false, "Producto personalizado no encontrado", 404);

            // Validar opciones
            var optionIds = dto.PersonalizationDetails.Select(x => x.OptionId).Distinct().ToList();
            var optionsInCatalog = await uow.Options.GetWhereAsync(x => optionIds.Contains(x.OptionId), ct);

            if (optionIds.Count != optionsInCatalog.Count())
            {
                var missingIds = optionIds.Except(optionsInCatalog.Select(o => o.OptionId)).ToList();
                return Result<PersonalizedProductDto>.Failure(false,
                    $"Las siguientes opciones no existen: {string.Join(", ", missingIds)}", 400);
            }

            // Obtener inventario para precio base
            var inventory = await uow.Inventory.GetInventoryByProductId(product.ProductId!.Value, ct);
            if (inventory is null)
                return Result<PersonalizedProductDto>.Failure(false, "No se encontró el inventario del producto", 404);

            try
            {
                await uow.BeginTransactionAsync(ct);

                // Recalcular personalizaciones
                var optionsMap = optionsInCatalog.ToDictionary(o => o.OptionId);
                decimal optionsTotal = 0;

                product.Personalizations.Clear();

                foreach (var detail in dto.PersonalizationDetails)
                {
                    var option = optionsMap[detail.OptionId];
                    var subTotal = detail.Quantity * (option.Price ?? 0);
                    optionsTotal += subTotal;

                    product.Personalizations.Add(new Personalization
                    {
                        OptionId = detail.OptionId,
                        Quantity = detail.Quantity,
                        SalePrice = option.Price,
                        SubTotal = subTotal
                    });
                }

                product.Description = dto.Description;
                product.SalePrice = (inventory.SalePrice ?? 0) + optionsTotal;

                await uow.SaveChangesAsync(ct);

                var updated = await uow.PersonalizedProducts.GetByIdAsync(product.PersonalizedId, ct);
                await uow.CommitTransactionAsync(ct);

                return Result<PersonalizedProductDto>.Success(mapper.Map<PersonalizedProductDto>(updated), 200);
            }
            catch (Exception)
            {
                await uow.RollbackTransactionAsync(ct);
                return Result<PersonalizedProductDto>.Failure(false,
                    "Ocurrió un error al actualizar el producto personalizado", 500);
            }
        }
    }
}