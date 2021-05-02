using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.Application.Model;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    public interface IContainerDetailApplicationService
    {
        Task<ContainerDetailModel> GetContainerDetailAsync(Guid containerId, CancellationToken ct);
    }
}