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
        Task<IEnumerable<Suppliers>> GetSuppliersAsync(FiltersDto dto, CancellationToken ct);
        Task<int> CountAsync(FiltersDto dto, CancellationToken ct);
        Task<Suppliers?> GetByIdAsync(int id, CancellationToken ct);
        Task<Suppliers?> GetByNameAsync(string name, CancellationToken ct);
        Task<Suppliers?> GetToUpdateAsync(int id, CancellationToken ct);
        Task<Suppliers> AddAsync(Suppliers supplier, CancellationToken ct);
        Task UpdateAsync(CancellationToken ct);
        Task DeactivateAsync(int id, CancellationToken ct);
    }
}
