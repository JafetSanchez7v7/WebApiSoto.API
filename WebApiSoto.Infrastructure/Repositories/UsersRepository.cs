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
        private readonly IMemoryCache _cache;
        public UsersRepository(AppDbContext con, IMemoryCache cache)
        {
            _context = con; 
            _cache = cache;
        }

        public async Task<IEnumerable<Users>>GetAsync(FiltersDto filters, CancellationToken ct)
        {
            if(!_cache.TryGetValue(UserKeys.CacheVersion, out int cacheVer))
            {
                cacheVer = 1;
                _cache.Set(UserKeys.CacheVersion, cacheVer);

            }
            var cacheKey = $"v{cacheVer}_{UserKeys.UserListKey}_{filters.PageNumber}_{filters.PageSize}_{filters.Name}_{filters.IsActive}";
            if (_cache.TryGetValue(cacheKey, out  List<Users>? cached))
             {
                return cached;
             }
          
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
           

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(cacheKey, items, cacheEntryOptions);

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
            var key = $"{UserKeys.UserIdKey}{id}";
            if(!_cache.TryGetValue(key, out Users? cachedUser))
            {
                var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id, ct);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(key, User, cacheEntryOptions);
            }

            return cachedUser;
        }

        public async Task<Users> AddAsync(Users user, CancellationToken ct)
        {
            var entry = await _context.Users.AddAsync(user, ct);
            InvalidateCache();
            return entry.Entity;
        }

        public async Task UpdateAsync(CancellationToken ct)
        {  
           await _context.SaveChangesAsync(ct);
            InvalidateCache();
        }

         public async Task<Users>GetByNameAsync(string name, CancellationToken ct)
        {
            var key = $"{UserKeys.UserNameKey}{name}";
            if(!_cache.TryGetValue(key, out Users cachedUser))
            {
                cachedUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == name);
                var cacheOptions = new MemoryCacheEntryOptions().
                    SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                if (cachedUser is not null)
                {
                    _cache.Set(key, cachedUser, cacheOptions);
                }
            }
            return cachedUser;
        }

        public void InvalidateCache()
        {
            if (_cache.TryGetValue(UserKeys.CacheVersion, out int cacheVer))
            {
                cacheVer++;
                _cache.Set(UserKeys.CacheVersion, cacheVer);
            }
        }
        
        public async Task DeactivateAsync(int id, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id, ct);
            if (user is not null)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync(ct);
                InvalidateCache();
            }
        }

        public async Task<Users?> GetToUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id, ct);
        }


    }
}
