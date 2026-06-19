using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.CacheK.UserKeys;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {

        private readonly AppDbContext _context;
        public UsersRepository(AppDbContext con)
        {
            _context = con; 
           
        }

        public async Task<IEnumerable<Users>>GetAsync(FiltersDto filters, CancellationToken ct)
        {
           
          
                var query = _context.Users.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(filters.Name))
                {
                    query = query.Where(u => u.UserName.Contains(filters.Name));
                }
                if (filters.IsActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == filters.IsActive.Value);
                }
                var items = await query
                    .Skip((filters.PageNumber - 1) * filters.PageSize)
                    .Take(filters.PageSize)
                    .ToListAsync(ct);
           

               

                return items;

        }

        public async Task<int> CountAsync(FiltersDto filters, CancellationToken ct)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(u => u.UserName.Contains(filters.Name));
            }
            if (filters.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filters.IsActive.Value);
            }
            return await query.CountAsync(ct);
        }

        public async Task<Users?>GetByIdAsync(int id, CancellationToken ct)
        {   
             return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id, ct);
      
        }

        public async Task<Users> AddAsync(Users user, CancellationToken ct)
        {
            var entry = await _context.Users.AddAsync(user, ct);
           
            return entry.Entity;
        }

        public async Task UpdateAsync(CancellationToken ct)
        {  
           await _context.SaveChangesAsync(ct);
        
        }

         public async Task<Users>GetByNameAsync(string name, CancellationToken ct)
        {
           
              return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == name);
              
        }

      
        public async Task DeactivateAsync(int id, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id, ct);
            if (user is not null)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync(ct);
               
            }
        }

        public async Task<Users?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Users.FindAsync(id);
        }


    }
}
