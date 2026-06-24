using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IPersonalizedProductRepository
    {
        Task<PersonalizedProduct?> GetByIdAsync(int id, CancellationToken ct);
        Task<PersonalizedProduct> AddAsync(PersonalizedProduct product, CancellationToken ct);

        Task<IEnumerable<PersonalizedProduct>> GetAllAsync(FiltersDto dto, CancellationToken ct);

        Task<PersonalizedProduct?> GetToUpdateAsync(int id, CancellationToken ct);

        Task<int> CountAsync(FiltersDto dto, CancellationToken ct);
    }
}
