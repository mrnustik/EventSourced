using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Application.Services.WarehouseItem;

namespace EventSourced.Sample.Warehouse.Web.Pages.Items.List
{
    public class ListViewModel : MasterPageViewModel
    {
        private readonly IGetAllWarehouseItemsApplicationService _getAllWarehouseItemsApplicationService;

        public ListViewModel(IGetAllWarehouseItemsApplicationService getAllWarehouseItemsApplicationService)
        {
            _getAllWarehouseItemsApplicationService = getAllWarehouseItemsApplicationService;
        }

        [Bind(Direction.ServerToClient)]
        public ICollection<WarehouseLisItemModel> WarehouseListItems { get; set; } = new List<WarehouseLisItemModel>();

        public override async Task PreRender()
        {
            await base.PreRender();
            if (!Context.IsPostBack)
            {
                WarehouseListItems = await _getAllWarehouseItemsApplicationService.GetAllAsync(RequestCancellationToken);
            }
        }
    }
}