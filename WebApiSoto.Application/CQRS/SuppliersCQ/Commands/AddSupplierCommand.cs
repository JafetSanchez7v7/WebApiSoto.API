using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.CQRS.SuppliersCQ.Commands
{
    public record AddSupplierCommand(CreateSupplierDto Dto) : IRequest<Result<SupplierDto>>;

    public class AddSupplierHandler : IRequestHandler<AddSupplierCommand, Result<SupplierDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AddSupplierHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<SupplierDto>> Handle(AddSupplierCommand request, CancellationToken ct)
        {
            var exists = await _uow.Supplier.GetByNameAsync(request.Dto.Name, ct);
            if (exists is not null)
                return Result<SupplierDto>.Failure(true, "Supplier already exists", 409);

            var entity = _mapper.Map<Suppliers>(request.Dto);
            await _uow.Supplier.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            var dto = _mapper.Map<SupplierDto>(entity);
            return Result<SupplierDto>.Success(dto, 201);
        }
    }
}
