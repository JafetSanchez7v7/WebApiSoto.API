using WebApiSoto.Infrastructure.Context;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;


public class CustomerRepository: ICustomersRepository
{
    private readonly AppDbContext _context;
    public CustomerRepository(AppDbContext con)
    {
        _context = con;
    }

    public async Task<IEnumerable<Customers>>GetCustomersAsync(FiltersDto dto, CancellationToken ct)
    {
        var query = _context.Customers.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(dto.Name))
        {
            query = query.Where(c => c.CustomerName.Contains(dto.Name));
        }
        if(dto.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == dto.IsActive.Value);
        }
        var response =await query.Skip((dto.PageNumber-1) * dto.PageSize).Take(dto.PageSize).ToListAsync(ct);
        return response;
    }

    public async Task<int> CountAsync(FiltersDto dto, CancellationToken ct)
    {
        var query = _context.Customers.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(dto.Name))
        {
            query = query.Where(c => c.CustomerName.Contains(dto.Name));
        }
        if(dto.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == dto.IsActive.Value);
        }
        
        return await query.CountAsync(ct);
    }

    public async Task<Customers?>GetByDNI(string dNI, CancellationToken ct)
    {
        return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Cedula == dNI);
    }

    public async Task<Customers?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == id, ct);
    }

    public async Task<Customers?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerName == name, ct);
    }

    public async Task<Customers?> GetToUpdateAsync(int id, CancellationToken ct)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id, ct);
    }

    public async Task<Customers>AddAsync(Customers customer, CancellationToken ct)
    {
         var result = await _context.Customers.AddAsync(customer, ct);
        
        return result.Entity;
    }

    public async Task UpdateAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeactivateAsync(int id, CancellationToken ct)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id, ct);
        if (customer is not null)
        {
            customer.IsActive = false;
            await _context.SaveChangesAsync(ct);
        }
    }
}