using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Domain.Container.Services
{
    public interface IMoveItemBetweenContainersService
    {
        Task MoveItemBetweenContainersAsync(Guid sourceContainerId, Guid destinationContainerId, Guid warehouseItemId, int amount, CancellationToken ct);
    }
}