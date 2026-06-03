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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync(FiltersDto dto, CancellationToken ct)
        {
            var query = _context.Categories.AsNoTracking().AsQueryable();

            // Aplicar filtros correctamente (usar asignación)
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

        public async Task<int> CountAsync(FiltersDto dto, CancellationToken ct)
        {
            var query = _context.Categories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name.Contains(dto.Name));
            }
            if (dto.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == dto.IsActive.Value);
            }

            return await query.CountAsync(ct);
        }

        public async Task<Categories?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryId == id, ct);
        }

        public async Task<Categories?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<Categories?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            // Devuelve la entidad en seguimiento para actualizar por tracking
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
        }

        public async Task<Categories> AddAsync(Categories category, CancellationToken ct)
        {
            var entry = await _context.Categories.AddAsync(category, ct);
            return entry.Entity;
        }

        public async Task UpdateAsync(CancellationToken ct)
        {
            // Guardado centralizado (se usa UnitOfWork.SaveChangesAsync normalmente)
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeactivateAsync(int id, CancellationToken ct)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
            if (entity is null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync(ct);
            }
            
        }
    }
}
