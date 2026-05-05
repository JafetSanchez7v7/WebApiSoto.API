using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> GetAsync(FiltersDto filters, CancellationToken ct);
        Task<Users?> GetByIdAsync (int id, CancellationToken ct);
        Task<Users?> GetToUpdateAsync(int id, CancellationToken ct);
        Task<Users> AddAsync(Users user, CancellationToken ct);
        Task UpdateAsync(CancellationToken ct);
        Task DeactivateAsync(int id, CancellationToken ct);

        Task<Users> GetByNameAsync(string name, CancellationToken ct);
  
    }
}
