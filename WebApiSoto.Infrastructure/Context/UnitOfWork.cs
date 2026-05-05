using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Infrastructure.Repositories;

namespace WebApiSoto.Infrastructure.Context
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
    
        
        public UnitOfWork(AppDbContext context, IServiceProvider ser )
        {
           _context = context;
            _serviceProvider = ser;
        }
        //PROPIEDADES DE NAVEGACION
        public ICustomersRepository Customers => _serviceProvider.GetRequiredService<ICustomersRepository>();
        public IUsersRepository User => _serviceProvider.GetRequiredService<IUsersRepository>();
        public ICategoryRepository Category => _serviceProvider.GetRequiredService<ICategoryRepository>();
        public ISupplierRepository Supplier => _serviceProvider.GetRequiredService<ISupplierRepository>();
        public IProductRepository ProductsI => _serviceProvider.GetRequiredService<IProductRepository>();

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct);
        }

    }
}
