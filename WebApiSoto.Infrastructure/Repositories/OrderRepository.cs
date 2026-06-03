using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == id, ct);
        }
        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime start, DateTime end, FilterOrderDto dto, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .Where(x => x.OrderDate >= start && x.OrderDate <= end)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
        }

        public async Task AddAsync(Order order, CancellationToken ct)
        {
            await _context.Orders.AddAsync(order, ct);
        }

        public async Task<Order> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderId == id, ct);
        }
    }
}
