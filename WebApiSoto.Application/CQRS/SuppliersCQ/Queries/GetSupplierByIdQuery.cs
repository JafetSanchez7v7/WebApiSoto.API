using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Suppliers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.SuppliersCQ.Queries
{
    public record GetSupplierByIdQuery(int Id) : IRequest<Result<SupplierDto>>;

    public class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetSupplierByIdHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<SupplierDto>> Handle(GetSupplierByIdQuery request, CancellationToken ct)
        {
            var supplier = await _uow.Supplier.GetByIdAsync(request.Id, ct);
            if (supplier is null)
                return Result<SupplierDto>.Failure(false, "Supplier not found", 404);

            var mapped = _mapper.Map<SupplierDto>(supplier);
            return Result<SupplierDto>.Success(mapped, 200);
        }
    }
}
