using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.API.Requests.WarehouseItem;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Application.Services.WarehouseItem;
using Microsoft.AspNetCore.Mvc;

namespace EventSourced.Sample.Warehouse.API.Controllers
{
    [ApiController]
    public class WarehouseItemController : ControllerBase
    {
        private readonly ICreateWarehouseItemApplicationService _createWarehouseItemApplicationService;

        private readonly IWarehouseItemDetailApplicationService _warehouseItemDetailApplicationService;
        private readonly IGetAllWarehouseItemsApplicationService _getAllWarehouseItemsApplicationService;

        public WarehouseItemController(ICreateWarehouseItemApplicationService createWarehouseItemApplicationService,
            IGetAllWarehouseItemsApplicationService getAllWarehouseItemsApplicationService,
            IWarehouseItemDetailApplicationService warehouseItemDetailApplicationService)
        {
            _createWarehouseItemApplicationService = createWarehouseItemApplicationService;
            _getAllWarehouseItemsApplicationService = getAllWarehouseItemsApplicationService;
            _warehouseItemDetailApplicationService = warehouseItemDetailApplicationService;
        }

        [HttpPost("create")]
        public Task Create([FromBody]
            CreateWarehouseItemRequest request,
            CancellationToken ct)
        {
            return _createWarehouseItemApplicationService.CreateWarehouseItemAsync(request.Title, ct);
        }

        [HttpGet("all")]
        public Task<ICollection<WarehouseLisItemModel>> GetAll(CancellationToken ct)
        {
            return _getAllWarehouseItemsApplicationService.GetAllAsync(ct);
        }
        
        [HttpGet("count")]
        public Task<int> GetCount(CancellationToken ct)
        {
            return _getAllWarehouseItemsApplicationService.GetCountAsync(ct);
        }
        
        [HttpGet("detail/{id:guid}")]
        public Task<WarehouseItemDetailModel> GetDetail(Guid id, CancellationToken ct)
        {
            return _warehouseItemDetailApplicationService.GetWarehouseItemDetailAsync(id, ct);
        }
    }
}