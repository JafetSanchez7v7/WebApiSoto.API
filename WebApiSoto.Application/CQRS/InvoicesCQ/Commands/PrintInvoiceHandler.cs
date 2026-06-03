using AutoMapper;
using MediatR;
using WebApiSoto.Application.Common.DTOs.Invoices;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;

namespace WebApiSoto.Application.CQRS.InvoicesCQ.Commands
{
    public record PrintInvoiceCommand() : IRequest<Result<InvoiceDto>>;

    public class PrintInvoiceHandler : IRequestHandler<PrintInvoiceCommand, Result<InvoiceDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PrintInvoiceHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<InvoiceDto>> Handle(PrintInvoiceCommand request, CancellationToken ct)
        {
            // aqui llamo el metodo para que se carguen las facturas antes ojala no de error
            await _uow.Invoice.GetUnprintedInvoicesAsync(ct);
            var response = await _uow.Invoice.PrintInvoiceAsync(ct);

            if (response is null)
                return Result<InvoiceDto>.Failure(false, "No invoices in queue to print", 404);

            var saved = await _uow.Invoice.GetInvoiceByIdAsync(response.InvoiceId, ct);
            var mapped = _mapper.Map<InvoiceDto>(saved);

            return Result<InvoiceDto>.Success(mapped, 200);
        }
    }
} 