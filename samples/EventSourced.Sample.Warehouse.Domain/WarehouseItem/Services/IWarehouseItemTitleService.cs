using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem.Services
{
    public interface IWarehouseItemTitleService
    {
        Task<string> GetWarehouseItemTitleAsync(Guid warehouseItemId, CancellationToken ct);
    }
}