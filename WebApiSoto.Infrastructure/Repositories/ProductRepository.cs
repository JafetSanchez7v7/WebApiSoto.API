using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Products>> GetAllAsync(FiltersDto dto, CancellationToken ct)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();
            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.ProductName.Contains(dto.Name));
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

        public async Task<Products?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id, ct);
        }

        public async Task<Products?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductName == name, ct);
        }

        public async Task<Products> AddAsync(Products entity, CancellationToken ct)
        {
            var entry = await _context.Products.AddAsync(entity, ct);
            return entry.Entity;
        }

        public async Task<Products?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id, ct);
        }

        public async Task<IEnumerable<Products>> GetWhereAsync(Expression<Func<Products, bool>> predicate, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(predicate)
                .ToListAsync(ct);
        }
    }
}
