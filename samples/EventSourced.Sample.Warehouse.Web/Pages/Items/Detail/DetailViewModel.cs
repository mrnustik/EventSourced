using System;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Services.WarehouseItem;

namespace EventSourced.Sample.Warehouse.Web.Pages.Items.Detail
{
    public class DetailViewModel : MasterPageViewModel
    {
        private readonly IWarehouseItemDetailApplicationService _warehouseItemDetailApplicationService;
        private readonly IImportWarehouseItemApplicationService _importWarehouseItemApplicationService;

        public DetailViewModel(IWarehouseItemDetailApplicationService warehouseItemDetailApplicationService, IImportWarehouseItemApplicationService importWarehouseItemApplicationService)
        {
            _warehouseItemDetailApplicationService = warehouseItemDetailApplicationService;
            _importWarehouseItemApplicationService = importWarehouseItemApplicationService;
        }

        [FromRoute(nameof(WarehouseItemId))]
        public Guid WarehouseItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Amount { get; set; }

        public override async Task Load()
        {
            var warehouseItemDetail = await _warehouseItemDetailApplicationService.GetWarehouseItemDetailAsync(WarehouseItemId, RequestCancellationToken);
            Title = warehouseItemDetail.Title;
            await base.Load();
        }

        [BusinessRuleExceptionFilter(nameof(Amount))]
        public async Task ImportAsync()
        {
            await _importWarehouseItemApplicationService.ImportWarehouseItemAsync(WarehouseItemId, Amount, RequestCancellationToken);
        }
    }
}