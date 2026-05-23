using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Interfaces
{
    public interface IUnitOfWork
    {
            IUsersRepository User { get; }
            ICategoryRepository Category { get; }
            ISupplierRepository Supplier { get; }
            IProductRepository ProductsI { get; }
            ICustomersRepository Customers { get; }
            Task<int>SaveChangesAsync(CancellationToken ct);

        Task BeginTransactionAsync(CancellationToken ct);
        Task CommitTransactionAsync(CancellationToken ct);
        Task RollbackTransactionAsync(CancellationToken ct);

     
    }
}
