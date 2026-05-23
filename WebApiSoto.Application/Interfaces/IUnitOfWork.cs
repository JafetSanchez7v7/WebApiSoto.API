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
            IOptionRepository Options { get; }
            IInventoryRepository Inventory { get; }
        Task<int>SaveChangesAsync(CancellationToken ct);
     
    }
}
