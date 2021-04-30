using System;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Application.Services.Container;
using EventSourced.Sample.Warehouse.Web.Models;

namespace EventSourced.Sample.Warehouse.Web.Pages.Container
{
    public class DetailViewModel : MasterPageViewModel
    {
        private readonly IContainerDetailApplicationService _containerDetailApplicationService;
        private readonly IConsumeItemsFromContainerApplicationService _consumeItemsFromContainerApplicationService;

        public DetailViewModel(IContainerDetailApplicationService containerDetailApplicationService, IConsumeItemsFromContainerApplicationService consumeItemsFromContainerApplicationService)
        {
            _containerDetailApplicationService = containerDetailApplicationService;
            _consumeItemsFromContainerApplicationService = consumeItemsFromContainerApplicationService;
        }
        
        [FromRoute(nameof(ContainerId))]
        public Guid ContainerId { get; set; }
        [Bind(Direction.ServerToClient)]
        public ContainerDetailModel ContainerDetailModel { get; set; }

        public ConsumeItemsDialogModel? ConsumeItemsDialogModel { get; set; }
        
        public override async Task Load()
        {
            await base.Load();
            await ReloadDataAsync();
        }

        private async Task ReloadDataAsync()
        {
            ContainerDetailModel = await _containerDetailApplicationService.GetContainerDetailAsync(ContainerId, RequestCancellationToken);
        }

        public void ShowConsumeItemsDialog()
        {
            ConsumeItemsDialogModel = new ConsumeItemsDialogModel(ContainerId,
                                                                  ContainerDetailModel
                                                                      .ContainerContents
                                                                      .Select(
                                                                          c => new ConsumeItemsDialogModel.ConsumeItemsDialogItemModel(
                                                                              c.WarehouseItemId,
                                                                              c.WarehouseItemName))
                                                                      .ToList(),
                                                                  0);
        }

        [BusinessRuleExceptionFilter(nameof(ConsumeItemsDialogModel))]
        public async Task ConsumeItemsAsync(ConsumeItemsDialogModel dialogModel)
        {
            await _consumeItemsFromContainerApplicationService.ConsumeItemsAsync(dialogModel.ContainerId, dialogModel.SelectedWarehouseItemId, dialogModel.Amount, RequestCancellationToken);
            await ReloadDataAsync();
        }
    }
}