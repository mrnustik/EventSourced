using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.Application.Model;

namespace EventSourced.Sample.Warehouse.Application.Services.ImportLocation
{
    public interface IGetImportLocationDataApplicationService
    {
        Task<ICollection<ImportLocationContentListItemModel>> GetImportLocationContentAsync(CancellationToken ct);
    }
}