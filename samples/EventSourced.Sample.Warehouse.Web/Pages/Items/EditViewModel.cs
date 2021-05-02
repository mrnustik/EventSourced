using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Services.WarehouseItem;

namespace EventSourced.Sample.Warehouse.Web.Pages.Items
{
    public class EditViewModel : MasterPageViewModel
    {
        private readonly IWarehouseItemDetailApplicationService _warehouseItemDetailApplicationService;
        private readonly IUpdateWarehouseItemApplicationService _updateWarehouseItemApplicationService;
        private readonly ICreateWarehouseItemApplicationService _createWarehouseItemApplicationService;

        public EditViewModel(IWarehouseItemDetailApplicationService warehouseItemDetailApplicationService,
                             IUpdateWarehouseItemApplicationService updateWarehouseItemApplicationService,
                             ICreateWarehouseItemApplicationService createWarehouseItemApplicationService)
        {
            _warehouseItemDetailApplicationService = warehouseItemDetailApplicationService;
            _updateWarehouseItemApplicationService = updateWarehouseItemApplicationService;
            _createWarehouseItemApplicationService = createWarehouseItemApplicationService;
        }

        [FromRoute(nameof(WarehouseItemId))]
        public Guid? WarehouseItemId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;

        public override async Task PreRender()
        {
            await base.PreRender();
            if (WarehouseItemId.HasValue)
            {
                var existingItem =
                    await _warehouseItemDetailApplicationService.GetWarehouseItemDetailAsync(
                        WarehouseItemId.Value,
                        RequestCancellationToken);
                Title = existingItem.Title;
            }
        }

        public async Task SaveAsync()
        {
            if (WarehouseItemId.HasValue)
            {
                await _updateWarehouseItemApplicationService.UpdateAsync(WarehouseItemId.Value, Title, RequestCancellationToken);
            }
            else
            {
                await _createWarehouseItemApplicationService.CreateWarehouseItemAsync(Title, RequestCancellationToken);
            }
            Context.RedirectToRoute("Pages/Items/List");
        }
    }
}