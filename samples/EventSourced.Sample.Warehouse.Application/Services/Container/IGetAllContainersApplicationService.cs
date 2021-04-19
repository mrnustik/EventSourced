using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.Application.Model;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    public interface IGetAllContainersApplicationService
    {
        Task<ICollection<ContainerListItemModel>> GetAllAsync(CancellationToken ct);
    }
}