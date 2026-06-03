using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.DTOs.Invoices;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.InvoicesCQ.Queries
{
    public record GetInvoiceByIdQuery(int Id) : IRequest<Result<InvoiceDto>>;
    public class GetByIdHandler : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetByIdHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Result<InvoiceDto>> Handle(GetInvoiceByIdQuery request, CancellationToken ct)
        {
            var response = await _uow.Invoice.GetInvoiceByIdAsync(request.Id, ct);
            if (response is null)
                return Result<InvoiceDto>.Failure(false, "Invoice not found", 404);
            var mapped = _mapper.Map<InvoiceDto>(response);
            return Result<InvoiceDto>.Success(mapped, 200);
        }
    }
}
