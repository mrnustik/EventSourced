using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.API.Requests.WarehouseItem;
using EventSourced.Sample.Warehouse.Application.Services.WarehouseItem;
using Microsoft.AspNetCore.Mvc;

namespace EventSourced.Sample.Warehouse.API.Controllers
{
    [ApiController]
    public class WarehouseItemController : ControllerBase
    {
        private readonly ICreateWarehouseItemApplicationService _createWarehouseItemApplicationService;

        public WarehouseItemController(ICreateWarehouseItemApplicationService createWarehouseItemApplicationService)
        {
            _createWarehouseItemApplicationService = createWarehouseItemApplicationService;
        }

        [HttpPost("create")]
        public Task Create([FromBody] CreateWarehouseItemRequest request,
                           CancellationToken ct)
        {
            return _createWarehouseItemApplicationService.CreateWarehouseItemAsync(request.Title, ct);
        }
    }
}