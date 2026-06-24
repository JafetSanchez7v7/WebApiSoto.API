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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private const int PersonalizedThreshold = 50000;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }


        private async Task CargarProductos(IEnumerable<Order> orders, CancellationToken ct)
        {
            var allDetails = orders.SelectMany(o => o.OrderDetails).ToList();

            var normalIds = allDetails
                .Where(d => d.ProductId < PersonalizedThreshold)
                .Select(d => d.ProductId)
                .Distinct()
                .ToList();

            var personalizedIds = allDetails
                .Where(d => d.ProductId >= PersonalizedThreshold)
                .Select(d => d.ProductId)
                .Distinct()
                .ToList();

            if (normalIds.Any())
            {
                var productos = await _context.Products
                    .AsNoTracking()
                    .Where(p => normalIds.Contains(p.ProductId))
                    .ToListAsync(ct);

                foreach (var detail in allDetails.Where(d => d.ProductId < PersonalizedThreshold))
                    detail.Product = productos.FirstOrDefault(p => p.ProductId == detail.ProductId);
            }

            if (personalizedIds.Any())
            {
                var personalizados = await _context.PersonalizedProducts
                    .AsNoTracking()
                    .Where(p => personalizedIds.Contains(p.PersonalizedId))
                    .ToListAsync(ct);

                foreach (var detail in allDetails.Where(d => d.ProductId >= PersonalizedThreshold))
                    detail.PersonalizedProduct = personalizados
                        .FirstOrDefault(p => p.PersonalizedId == detail.ProductId);
            }
        }

        // ── GetByIdAsync ──────────────────────────────────────────────────────
        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)   // sin ThenInclude(Product)
                .FirstOrDefaultAsync(x => x.OrderId == id, ct);

            if (order is not null)
                await CargarProductos(new[] { order }, ct);

            return order;
        }

        // ── GetByDateRangeAsync ───────────────────────────────────────────────
        public async Task<IEnumerable<Order>> GetByDateRangeAsync(
            DateTime start, DateTime end, FilterOrderDto dto, CancellationToken ct)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)   // sin ThenInclude(Product)
                .Where(x => x.OrderDate >= start && x.OrderDate <= end)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);

            await CargarProductos(orders, ct);
            return orders;
        }

        // ── AddAsync ──────────────────────────────────────────────────────────
        public async Task AddAsync(Order order, CancellationToken ct)
        {
            await _context.Orders.AddAsync(order, ct);
        }

        // ── GetToUpdateAsync ──────────────────────────────────────────────────
        public async Task<Order> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderId == id, ct);
        }

        // ── CountAsync ────────────────────────────────────────────────────────
        public async Task<int> CountAsync(FilterOrderDto dto, CancellationToken ct)
        {
            var query = _context.Orders.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.CustomerName))
                query = query.Where(x => x.Customer.CustomerName == dto.CustomerName);
            if (dto.Status.HasValue)
                query = query.Where(o => o.IsActive == dto.Status);
            if (dto.from.HasValue)
                query = query.Where(x => x.OrderDate >= dto.from.Value);
            if (dto.to.HasValue)
                query = query.Where(x => x.OrderDate <= dto.to.Value.AddDays(1).AddTicks(-1));

            return await query.CountAsync(ct);
        }

        // ── GetAll ────────────────────────────────────────────────────────────
        public async Task<IEnumerable<Order>> GetAll(FilterOrderDto dto, CancellationToken ct)
        {
            var query = _context.Orders.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.CustomerName))
                query = query.Where(x => x.Customer.CustomerName == dto.CustomerName);
            if (dto.Status.HasValue)
                query = query.Where(o => o.IsActive == dto.Status);
            if (dto.from.HasValue)
                query = query.Where(x => x.OrderDate >= dto.from.Value);
            if (dto.to.HasValue)
                query = query.Where(x => x.OrderDate <= dto.to.Value.AddDays(1).AddTicks(-1));

            var orders = await query
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)   
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);

            await CargarProductos(orders, ct);
            return orders;
        }
    }
}