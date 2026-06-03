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
    public record GetInvoicesQuery(FilterSalesDto Dto):IRequest<Result<PaginationList<InvoiceDto>>>;
    public class GetAllInvoicesHandler(IUnitOfWork unitOfWork, IMapper mapper): IRequestHandler<GetInvoicesQuery, Result<PaginationList<InvoiceDto>>>
    {
        public async Task<Result<PaginationList<InvoiceDto>>> Handle(GetInvoicesQuery request, CancellationToken ct)
        {
            var result =  await unitOfWork.Invoice.GetAllAsync(request.Dto,  ct);
            if(!result.Any())
                return Result<PaginationList<InvoiceDto>>.Failure(true, "No se encontraron facturas", 200);
            var totalCount = await unitOfWork.Invoice.CountAsync(request.Dto, ct);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.Dto.PageSize);

            var mapped = mapper.Map<List<InvoiceDto>>(result);

            var pagination = new PaginationList<InvoiceDto>(mapped, request.Dto.PageNumber, totalPages, totalCount);

            return Result<PaginationList<InvoiceDto>>.Success(pagination, 200);

        }
    }
}
