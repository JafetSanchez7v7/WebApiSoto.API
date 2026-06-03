using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync( FiltersDto dto, CancellationToken ct);
        Task<int> CountAsync(FiltersDto dto, CancellationToken ct);
        Task<Products?> GetByIdAsync(int id, CancellationToken ct);
        Task<Products?> GetByNameAsync(string name, CancellationToken ct);
        Task<Products> AddAsync(Products entity, CancellationToken ct);
        Task<Products?> GetToUpdateAsync(int id, CancellationToken ct);
        Task<IEnumerable<Products>> GetWhereAsync(Expression<Func<Products, bool>> predicate, CancellationToken ct);
    }
}
