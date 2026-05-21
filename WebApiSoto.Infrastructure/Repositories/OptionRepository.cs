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
    public class OptionRepository : IOptionRepository
    {
        private readonly AppDbContext _context;
        public OptionRepository(AppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Option>>GetOptions(FIlterOptionsDto dto, CancellationToken ct)
        {
           var query = _context.Options.AsNoTracking().AsQueryable();
           
            //filtros
            if(!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name.Contains(dto.Name));
            }
            if(dto.PriceGreaterThan.HasValue)
            {
                query = query.Where(x => x.Price > dto.PriceGreaterThan.Value);
            }

            var lista= await query.Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(ct);
             return lista;

            

        }

      
    }
}
