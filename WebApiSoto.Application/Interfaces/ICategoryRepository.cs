using WebApiSoto.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;


namespace WebApiSoto.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Categories>> GetCategoriesAsync(FiltersDto dto, CancellationToken ct);
        Task<Categories?> GetByIdAsync(int id, CancellationToken ct);
        Task<Categories?> GetByNameAsync(string name, CancellationToken ct);
        Task<Categories?> GetToUpdateAsync(int id, CancellationToken ct);

        // Nuevos m�todos para CRUD / patch
        Task<Categories> AddAsync(Categories category, CancellationToken ct);
        Task UpdateAsync(CancellationToken ct);
        Task DeactivateAsync(int id, CancellationToken ct);
    }
}