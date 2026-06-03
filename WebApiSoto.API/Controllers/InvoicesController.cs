using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.CQRS.InvoicesCQ.Commands;
using WebApiSoto.Application.CQRS.InvoicesCQ.Queries;

namespace WebApiSoto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Gerent,Operator")]
    public class InvoicesController : ApiController
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] FilterSalesDto dto, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetInvoicesQuery(dto), ct);
            return HandleResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetInvoiceByIdQuery(id), ct);
            return HandleResult(response);
        }

        [HttpGet("unprinted")]
        public async Task<IActionResult> GetUnprintedInvoices( CancellationToken ct)
        {
            var response = await _mediator.Send(new GetUnprintedInvoicesQuery(), ct);
            return HandleResult(response);
        }

        [HttpPatch("print")]
        public async Task<IActionResult> PrintInvoice(CancellationToken ct)
        {
            var response = await _mediator.Send(new PrintInvoiceCommand(), ct);
            return HandleResult(response);
        }
    }
}