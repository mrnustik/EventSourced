using System;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Application.Services.Container;

namespace EventSourced.Sample.Warehouse.Web.Pages.Container
{
    public class DetailViewModel : MasterPageViewModel
    {
        private readonly IContainerDetailApplicationService _containerDetailApplicationService;

        public DetailViewModel(IContainerDetailApplicationService containerDetailApplicationService)
        {
            _containerDetailApplicationService = containerDetailApplicationService;
        }
        
        [FromRoute(nameof(ContainerId))]
        public Guid ContainerId { get; set; }
        [Bind(Direction.ServerToClient)]
        public ContainerDetailModel ContainerDetailModel { get; set; }

        public override async Task PreRender()
        {
            await base.PreRender();
            ContainerDetailModel =
                await _containerDetailApplicationService.GetContainerDetailAsync(ContainerId, RequestCancellationToken);
        }
    }
}