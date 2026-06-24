using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Necesario para el CreateScope
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentQueue<Invoice> _invoiceQueue;

        // Constructor limpio, solo recibe la fábrica de alcances para la BD
        public InvoiceRepository(IServiceScopeFactory scopeFactory)
        {
            
            _scopeFactory = scopeFactory;
            _invoiceQueue = new ConcurrentQueue<Invoice>();
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync(FilterSalesDto dto, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var query = context.Invoices.AsNoTracking().Include(d => d.InvoiceDetails)
                .ThenInclude(d => d.Product).AsQueryable();

            // Aplicacion de los filtros
            if (!string.IsNullOrEmpty(dto.CustomerName))
                query = query.Where(i => i.Sale.Customer.CustomerName == dto.CustomerName);
            if (dto.MaxTotal.HasValue)
                query = query.Where(i => i.TotalAmount <= dto.MaxTotal.Value);
            if (dto.MinTotal.HasValue)
                query = query.Where(i => i.TotalAmount >= dto.MinTotal.Value);
            if (dto.from.HasValue)
                query = query.Where(x => x.CreateDate >= dto.from.Value);
            if (dto.to.HasValue)
                query = query.Where(x => x.CreateDate <= dto.to.Value.AddDays(1).AddTicks(-1));

            return await query.Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(ct);
        }

        public void EnQueue(List<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                _invoiceQueue.Enqueue(invoice);
            }
        }

        public async Task<ConcurrentQueue<Invoice>> GetUnprintedInvoicesAsync(CancellationToken ct)
        {
            _invoiceQueue.Clear(); // Limpiar la cola antes de cargar nuevos datos 

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var list = await context.Invoices.AsNoTracking()
                .Include(d => d.InvoiceDetails)
                .ThenInclude(d => d.Product)
                .Where(i => i.IsPrinted == false)
                .OrderBy(i => i.CreateDate)
               .ToListAsync(ct);

            EnQueue(list);
            return _invoiceQueue;
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await context.Invoices.AsNoTracking()
                .Include(d => d.InvoiceDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(i => i.InvoiceId == id, ct);
        }

       

        public async Task<Invoice> GetToUpdate(int id, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == id, ct);
        }

        public async Task<Invoice> PrintInvoiceAsync(CancellationToken ct)
        {
            if (_invoiceQueue.TryDequeue(out var invoice))
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var invoiceToUpdate = await context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == invoice.InvoiceId, ct);

                if (invoiceToUpdate != null)
                {
                    invoiceToUpdate.IsPrinted = true;
                    invoiceToUpdate.PrintedDate = DateTime.UtcNow;
                    await context.SaveChangesAsync(ct);
                    return invoiceToUpdate;
                }
            }
            return null;
        }

        public async Task<int> CountAsync(FilterSalesDto dto, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var query = context.Invoices.AsNoTracking().AsQueryable();

            // Aplicacion de los filtros
            if (!string.IsNullOrEmpty(dto.CustomerName))
                query = query.Where(i => i.Sale.Customer.CustomerName == dto.CustomerName);
            if (dto.MaxTotal.HasValue)
                query = query.Where(i => i.TotalAmount <= dto.MaxTotal.Value);
            if (dto.MinTotal.HasValue)
                query = query.Where(i => i.TotalAmount >= dto.MinTotal.Value);
            if (dto.from.HasValue)
                query = query.Where(x => x.CreateDate >= dto.from.Value);
            if (dto.to.HasValue)
                query = query.Where(x => x.CreateDate <= dto.to.Value.AddDays(1).AddTicks(-1));

            return await query.CountAsync(ct);
        }
    }
}