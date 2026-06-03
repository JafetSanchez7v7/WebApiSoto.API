using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IPurchasesRepository
    {
        Task<Purchase> AddPurchaseAsync(Purchase purchase, CancellationToken ct);
        Task<Purchase> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<Purchase>> GetPurchasesAsync(FilterPurchasesDto dto, CancellationToken ct);
        Task<int> CountAsync(FilterPurchasesDto dto, CancellationToken ct);
    }
}
