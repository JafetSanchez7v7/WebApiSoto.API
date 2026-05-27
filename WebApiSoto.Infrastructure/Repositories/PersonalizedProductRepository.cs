using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                .Include(x => x.Personalizations)
                    .ThenInclude(x => x.Option)
                .FirstOrDefaultAsync(x => x.PersonalizedId == id, ct);
        }

        public async Task<PersonalizedProduct> AddAsync(PersonalizedProduct product, CancellationToken ct)
        {
           var result= await _context.PersonalizedProducts.AddAsync(product, ct);
            return result.Entity;
        }
    }
}
