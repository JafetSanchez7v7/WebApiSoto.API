using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Infrastructure.Repositories;

namespace WebApiSoto.Infrastructure.Context
{
    public class UnitOfWork: IUnitOfWork
    {
        private IDbContextTransaction? _currentTransaction;
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
        public IOptionRepository Options => _serviceProvider.GetRequiredService<IOptionRepository>();
        public IInventoryRepository Inventory => _serviceProvider.GetRequiredService<IInventoryRepository>();
        public IPurchasesRepository Purchases => _serviceProvider.GetRequiredService<IPurchasesRepository>();
        public ISalesRepository Sales => _serviceProvider.GetRequiredService<ISalesRepository>();
        public IPersonalizedProductRepository PersonalizedProducts =>_serviceProvider.GetRequiredService<IPersonalizedProductRepository>();
        public IOrderRepository Orders => _serviceProvider.GetRequiredService<IOrderRepository>();
        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task BeginTransactionAsync(CancellationToken ct)
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct)
        {
            try
            {
                await SaveChangesAsync(ct);

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync(ct);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken ct)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

    }
}
