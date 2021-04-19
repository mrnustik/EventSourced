using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Application.Services.Container;

namespace EventSourced.Sample.Warehouse.Web.Pages.Container
{
    public class ListViewModel : MasterPageViewModel
    {
        private readonly IGetAllContainersApplicationService _getAllContainersApplicationService;

        public ListViewModel(IGetAllContainersApplicationService getAllContainersApplicationService)
        {
            _getAllContainersApplicationService = getAllContainersApplicationService;
        }

        [Bind(Direction.ServerToClient)]
        public ICollection<ContainerListItemModel> Containers { get; set; } = new List<ContainerListItemModel>();

        public override async Task PreRender()
        {
            await base.PreRender();
            if (!Context.IsPostBack)
            {
                Containers = await _getAllContainersApplicationService.GetAllAsync(RequestCancellationToken);
            }
        }
    }
}