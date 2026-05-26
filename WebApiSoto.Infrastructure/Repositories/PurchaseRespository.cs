using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class PurchaseRespository(AppDbContext _context) : IPurchasesRepository
    {
      
        public async Task<IEnumerable<Purchase>> GetPurchasesAsync(FilterPurchasesDto dto, CancellationToken ct)
        {
            var query = _context.Purchases.AsNoTracking()
                .Include(x => x.Suppliers)
                .Include(x => x.PurchaseDetails)
                .ThenInclude(z => z.Products)
                .AsQueryable();

            if (dto.SupplierId.HasValue && dto.SupplierId > 0)
                query = query.Where(x => x.SupplierId == dto.SupplierId);

            if (dto.MinTotal.HasValue && dto.MinTotal > 0)
                query = query.Where(x => x.TotalAmount >= dto.MinTotal.Value);

            if (dto.MaxTotal.HasValue && dto.MaxTotal > 0)
                query = query.Where(x => x.TotalAmount <= dto.MaxTotal.Value);

            if (dto.from.HasValue)
                query = query.Where(x => x.Date >= dto.from.Value);

            if (dto.to.HasValue)
                query = query.Where(x => x.Date <= dto.to.Value.AddDays(1).AddTicks(-1));


            return await query
                .OrderByDescending(x => x.Date)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
            
        }
        public async Task<Purchase> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Purchases.AsNoTracking().Include(x => x.Suppliers).Include(x => x.PurchaseDetails).ThenInclude(x => x.Products).FirstOrDefaultAsync(x => x.PurchaseId == id, ct);
        }

        public async Task<Purchase> AddPurchaseAsync(Purchase purchase, CancellationToken ct)
        {
            var added = await _context.AddAsync(purchase, ct);
            return added.Entity;
        }

    }

    
}
