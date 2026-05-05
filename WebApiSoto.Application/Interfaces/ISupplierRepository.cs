using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetSuppliersAsync(FiltersDto dto, CancellationToken ct);
        Task<Supplier?> GetByIdAsync(int id, CancellationToken ct);
        Task<Supplier?> GetByNameAsync(string name, CancellationToken ct);
        Task<Supplier?> GetToUpdateAsync(int id, CancellationToken ct);
        Task<Supplier> AddAsync(Supplier supplier, CancellationToken ct);
        Task UpdateAsync(CancellationToken ct);
        Task DeactivateAsync(int id, CancellationToken ct);
    }
}
