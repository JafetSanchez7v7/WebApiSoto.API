using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime start, DateTime end, FilterOrderDto dto, CancellationToken ct);
        Task AddAsync(Order order, CancellationToken ct);
        Task<Order?> GetToUpdateAsync(int id, CancellationToken ct);

    }
}
