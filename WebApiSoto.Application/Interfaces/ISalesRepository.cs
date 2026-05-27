using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface ISalesRepository
    {
        Task<Sale> AddSaleAsync(Sale sale, CancellationToken ct);
        Task<Sale?> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<Sale>> GetSalesAsync(FilterSalesDto dto, CancellationToken ct);
    }
}
