using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface ICustomersRepository
    {
        Task<IEnumerable<Customers>> GetCustomersAsync(FiltersDto dto, CancellationToken ct);
        Task<int> CountAsync(FiltersDto dto, CancellationToken ct);
        Task<Customers?> GetByIdAsync(int id, CancellationToken ct);
        Task<Customers?> GetByNameAsync(string name, CancellationToken ct);
        Task<Customers?> GetToUpdateAsync(int id, CancellationToken ct);

        // Nuevos métodos para CRUD / patch
        Task<Customers> AddAsync(Customers customer, CancellationToken ct);
        Task UpdateAsync(CancellationToken ct);
        Task DeactivateAsync(int id, CancellationToken ct);
    }

}
