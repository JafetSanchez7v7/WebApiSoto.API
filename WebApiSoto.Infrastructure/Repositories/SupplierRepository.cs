using Microsoft.EntityFrameworkCore;
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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Suppliers>> GetSuppliersAsync(FiltersDto dto, CancellationToken ct)
        {
            var query = _context.Suppliers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name.Contains(dto.Name));
            }
            if (dto.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == dto.IsActive.Value);
            }

            var response = await query
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);

            return response;
        }

        public async Task<Suppliers?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SupplierId == id, ct);
        }

        public async Task<Suppliers?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<Suppliers?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
        }

        public async Task<Suppliers> AddAsync(Suppliers supplier, CancellationToken ct)
        {
            var entry = await _context.Suppliers.AddAsync(supplier, ct);
            return entry.Entity;
        }

        public async Task UpdateAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeactivateAsync(int id, CancellationToken ct)
        {
            var entity = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id, ct);
            if (entity is not null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
