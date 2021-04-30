using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    public interface IMoveItemsBetweenContainersApplicationService
    {
        Task MoveItemBetweenContainersAsync(Guid sourceContainerId,
                                            Guid destinationContainer,
                                            Guid warehouseItemId,
                                            int amount,
                                            CancellationToken ct);
    }
}