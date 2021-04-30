using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    public interface IConsumeItemsFromContainerApplicationService
    {
        Task ConsumeItemsAsync(Guid containerId, Guid warehouseItemId, int amount, CancellationToken ct);
    }
}