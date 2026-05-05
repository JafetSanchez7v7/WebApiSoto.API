using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.SuppliersCQ.Commands
{
    public record UpdateSupplierCommand(int Id, UpdateSupplierDto Dto) : IRequest<Result<SupplierDto>>;

    public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, Result<SupplierDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateSupplierHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken ct)
        {
            var entity = await _uow.Supplier.GetToUpdateAsync(request.Id, ct);
            if (entity is null)
                return Result<SupplierDto>.Failure(true, "Supplier not found", 404);

            _mapper.Map(request.Dto, entity);
            await _uow.SaveChangesAsync(ct);

            var dto = _mapper.Map<SupplierDto>(entity);
            return Result<SupplierDto>.Success(dto, 200);
        }
    }
}
