using System;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Services.Container;

namespace EventSourced.Sample.Warehouse.Web.Pages.Container
{
    public class EditViewModel : MasterPageViewModel
    {
        private readonly ICreateContainerApplicationService _createContainerApplicationService;

        public EditViewModel(ICreateContainerApplicationService createContainerApplicationService)
        {
            _createContainerApplicationService = createContainerApplicationService;
        }

        [FromRoute(nameof(ContainerId))]
        public Guid? ContainerId { get; set; }
        
        public string Identifier { get; set; } = string.Empty;

        public async Task SaveAsync()
        {
            if (ContainerId != null)
            {
                //TODO edit container
            }
            else
            {
                await _createContainerApplicationService.CreateContainerAsync(Identifier, RequestCancellationToken);
            }
            Context.RedirectToRoute("Pages/Container/List");
        }
    }
}