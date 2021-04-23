using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Application.Services.Container;
using EventSourced.Sample.Warehouse.Application.Services.ImportLocation;

namespace EventSourced.Sample.Warehouse.Web.Pages.ImportLocation
{
    public class DetailViewModel : MasterPageViewModel
    {
        private readonly IGetImportLocationDataApplicationService _getImportLocationDataApplicationService;
        private readonly IGetAllContainersApplicationService _getAllContainersApplicationService;
        private readonly IMoveFromImportLocationToContainerApplicationService _moveFromImportLocationToContainerApplicationService;

        public DetailViewModel(IGetImportLocationDataApplicationService getImportLocationDataApplicationService,
                               IGetAllContainersApplicationService getAllContainersApplicationService,
                               IMoveFromImportLocationToContainerApplicationService moveFromImportLocationToContainerApplicationService)
        {
            _getImportLocationDataApplicationService = getImportLocationDataApplicationService;
            _getAllContainersApplicationService = getAllContainersApplicationService;
            _moveFromImportLocationToContainerApplicationService = moveFromImportLocationToContainerApplicationService;
        }

        [Bind(Direction.ServerToClient)]
        public ICollection<ImportLocationContentListItemModel> ImportLocationContents { get; set; } =
            new List<ImportLocationContentListItemModel>();

        [Bind(Direction.ServerToClient)]
        public ICollection<ContainerListItemModel> Containers { get; set; } = 
            new List<ContainerListItemModel>();

        public Guid SelectedWarehouseItemId { get; set; }
        public Guid SelectedContainerId { get; set; }
        public int Amount { get; set; }

        public override async Task PreRender()
        {
            ImportLocationContents = await _getImportLocationDataApplicationService.GetImportLocationContentAsync(RequestCancellationToken);
            Containers = await _getAllContainersApplicationService.GetAllAsync(RequestCancellationToken);
            await base.PreRender();
        }

        [BusinessRuleExceptionFilter(nameof(SelectedWarehouseItemId))]
        public Task MoveToContainer()
        {
            return _moveFromImportLocationToContainerApplicationService.MoveFromImportLocationToContainerAsync(
                SelectedContainerId,
                SelectedWarehouseItemId,
                Amount,
                RequestCancellationToken);
        }
    }
}