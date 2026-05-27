using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Inventory>> GetInventory(FilterInventoryDto dto, CancellationToken ct)
        {
            var query = _context.Inventories
                .AsNoTracking()
                .Include(x => x.Product)
                .AsQueryable();

            if (dto.Id.HasValue)
                query = query.Where(x => x.InventoryId == dto.Id.Value);

            if (dto.ProductId.HasValue)
                query = query.Where(x => x.ProductId == dto.ProductId.Value);

            if (!string.IsNullOrEmpty(dto.ProductName))
                query = query.Where(x => x.Product != null && x.Product.ProductName.Contains(dto.ProductName));

            return await query
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
        }

        public async Task<Inventory?> GetInventoryById(int id, CancellationToken ct)
        {
            return await _context.Inventories
                .AsNoTracking()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.InventoryId == id, ct);
        }

        public async Task<Inventory?> GetInventoryByProductId(int productId, CancellationToken ct)
        {
            return await _context.Inventories
                .AsNoTracking()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ProductId == productId, ct);
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByProductName(string productName, CancellationToken ct)
        {
            return await _context.Inventories
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.Product != null && x.Product.ProductName.Contains(productName))
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Inventory>>GetWhereAsync(Expression<Func<Inventory,bool>> predicate, CancellationToken ct)
        {
            return await _context.Inventories.Include(x => x.Product).Where(predicate).ToListAsync(ct);
        }
        public async Task<Inventory> AddAsync(Inventory inv, CancellationToken ct)
        {
            var result = await _context.Inventories.AddAsync(inv, ct);
            return result.Entity;
        }

        public async Task<Inventory?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(x => x.InventoryId == id, ct);
        }
    }
}
