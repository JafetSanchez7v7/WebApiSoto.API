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
    public class OptionRepository : IOptionRepository
    {
        private readonly AppDbContext _context;
        public OptionRepository(AppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Option>> GetOptions(FIlterOptionsDto dto, CancellationToken ct)
        {
            var query = _context.Options.AsNoTracking().AsQueryable();

            //filtros
            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(dto.Name));
            }
            if (dto.PriceGreaterThan.HasValue)
            {
                query = query.Where(x => x.Price > dto.PriceGreaterThan.Value);
            }

            var lista = await query.Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
            return lista;
        }

        public async Task<int> CountAsync(FIlterOptionsDto dto, CancellationToken ct)
        {
            var query = _context.Options.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(dto.Name));
            }
            if (dto.PriceGreaterThan.HasValue)
            {
                query = query.Where(x => x.Price > dto.PriceGreaterThan.Value);
            }

            return await query.CountAsync(ct);
        }

        public async Task<Option> GetOptionById(int id, CancellationToken ct)
        {
            return await _context.Options
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OptionId == id, ct);
        }

        public async Task<Option> CreateOption(Option option, CancellationToken ct)
        {
            await _context.Options.AddAsync(option, ct);
            return option;
        }

        public async Task<Option> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Options
                .FirstOrDefaultAsync(x => x.OptionId == id, ct);
        }

        public async Task DeleteOption(int id, CancellationToken ct)
        {
            var option = await _context.Options.FindAsync(id, ct);
            if (option is not null)
                _context.Options.Remove(option);
        }

        public async Task<IEnumerable<Option>> GetWhereAsync(Expression<Func<Option, bool>> predicate, CancellationToken ct)
        {
            return await _context.Options
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(ct);
        }
    }

}
