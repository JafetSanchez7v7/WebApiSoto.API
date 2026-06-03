using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IPersonalizedProductRepository
    {
        Task<PersonalizedProduct?> GetByIdAsync(int id, CancellationToken ct);
        Task<PersonalizedProduct> AddAsync(PersonalizedProduct product, CancellationToken ct);
    }
}
