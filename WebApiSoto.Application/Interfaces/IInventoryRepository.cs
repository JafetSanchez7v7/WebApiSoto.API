using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetInventory(FilterInventoryDto dto, CancellationToken ct);
        Task<Inventory?> GetInventoryById(int id, CancellationToken ct);
        Task<Inventory?> GetInventoryByProductId(int productId, CancellationToken ct);
        Task<IEnumerable<Inventory>> GetInventoryByProductName(string productName, CancellationToken ct);

    }
}
