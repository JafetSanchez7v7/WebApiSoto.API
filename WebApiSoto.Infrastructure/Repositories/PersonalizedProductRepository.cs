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
    public class PersonalizedProductRepository: IPersonalizedProductRepository
    {
        private readonly AppDbContext _context;

        public PersonalizedProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PersonalizedProduct?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.PersonalizedProducts
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Products)
                .Include(x => x.Personalizations)
                    .ThenInclude(x => x.Option)
                .FirstOrDefaultAsync(x => x.PersonalizedId == id, ct);
        }

        public async Task<PersonalizedProduct> AddAsync(PersonalizedProduct product, CancellationToken ct)
        {
           var result= await _context.PersonalizedProducts.AddAsync(product, ct);
            return result.Entity;
        }

        public async Task<IEnumerable<PersonalizedProduct>> GetAllAsync(FiltersDto dto, CancellationToken ct)
        {
            var query = _context.PersonalizedProducts
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Products)
                .Include(x => x.Personalizations)
                    .ThenInclude(x => x.Option)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(x => x.Customer.CustomerName.Contains(dto.Name));

            return await query
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
        }

        public async Task<PersonalizedProduct?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.PersonalizedProducts
                .Include(x => x.Personalizations)
                .FirstOrDefaultAsync(x => x.PersonalizedId == id, ct);
        }

        public async Task<int> CountAsync(FiltersDto dto, CancellationToken ct)
        {
            var query = _context.PersonalizedProducts
                .AsNoTracking()
                .Include(x => x.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(x => x.Customer.CustomerName.Contains(dto.Name));

            return await query.CountAsync(ct);
        }
    }
}
