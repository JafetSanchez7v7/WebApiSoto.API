using AutoMapper;
using MediatR;
using System.Collections.Concurrent;
using WebApiSoto.Application.Common.DTOs.Invoices;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.InvoicesCQ.Queries
{
    public record GetUnprintedInvoicesQuery() : IRequest<Result<List<InvoiceDto>>>;

    public class GetUnprintedInvoicesHandler : IRequestHandler<GetUnprintedInvoicesQuery, Result<List<InvoiceDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUnprintedInvoicesHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<List<InvoiceDto>>> Handle(GetUnprintedInvoicesQuery request, CancellationToken ct)
        {
            var queue = await _uow.Invoice.GetUnprintedInvoicesAsync(ct);

            if (queue.IsEmpty)
                return Result<List<InvoiceDto>>.Failure(true, "No unprinted invoices found", 200);

            

            var mapped = _mapper.Map<List<InvoiceDto>>(queue);

            
            return Result<List<InvoiceDto>>.Success(mapped, 200);
        }
    }
}