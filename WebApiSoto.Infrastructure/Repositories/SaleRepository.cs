using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class SaleRepository(AppDbContext _context) : ISalesRepository
    {
        public async Task<IEnumerable<Sale>> GetSalesAsync(FilterSalesDto dto, CancellationToken ct)
        {
            var query = _context.Sales.AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.SaleDetails)
                .ThenInclude(z => z.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.CustomerName))
                query = query.Where(x => x.Customer.CustomerName == dto.CustomerName);

            if (dto.MinTotal.HasValue && dto.MinTotal > 0)
                query = query.Where(x => x.SaleTotal >= dto.MinTotal.Value);

            if (dto.MaxTotal.HasValue && dto.MaxTotal > 0)
                query = query.Where(x => x.SaleTotal <= dto.MaxTotal.Value);

            if (dto.from.HasValue)
                query = query.Where(x => x.SaleDate >= dto.from.Value);

            if (dto.to.HasValue)
                query = query.Where(x => x.SaleDate <= dto.to.Value.AddDays(1).AddTicks(-1));

            return await query
                .OrderByDescending(x => x.SaleDate)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
        }

        public async Task<Sale?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Sales.AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.SaleDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.SaleId == id, ct);
        }

        public async Task<Sale> AddSaleAsync(Sale sale, CancellationToken ct)
        {
            var added = await _context.AddAsync(sale, ct);
            return added.Entity;
        }

        public async Task<int> CountAsync(FilterSalesDto dto, CancellationToken ct)
        {
            var query = _context.Sales.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.CustomerName))
                query = query.Where(x => x.Customer.CustomerName == dto.CustomerName);

            if (dto.MinTotal.HasValue && dto.MinTotal > 0)
                query = query.Where(x => x.SaleTotal >= dto.MinTotal.Value);

            if (dto.MaxTotal.HasValue && dto.MaxTotal > 0)
                query = query.Where(x => x.SaleTotal <= dto.MaxTotal.Value);

            if (dto.from.HasValue)
                query = query.Where(x => x.SaleDate >= dto.from.Value);

            if (dto.to.HasValue)
                query = query.Where(x => x.SaleDate <= dto.to.Value.AddDays(1).AddTicks(-1));

            return await query.CountAsync(ct);

             
        }
    }
}
