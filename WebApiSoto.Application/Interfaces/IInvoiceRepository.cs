using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public  interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetAllAsync(FilterSalesDto dto, CancellationToken ct);
        Task<ConcurrentQueue<Invoice>> GetUnprintedInvoicesAsync( CancellationToken ct);
        Task<Invoice> GetInvoiceByIdAsync(int id, CancellationToken ct);

        Task<Invoice> PrintInvoiceAsync(CancellationToken ct);

        Task<int> CountAsync(FilterSalesDto dto, CancellationToken ct);
    }
}
