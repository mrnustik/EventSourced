using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Configuration;
using EventSourced.Diagnostics.Web.Model.ExternalEvents;
using EventSourced.Diagnostics.Web.Services;

namespace EventSourced.Diagnostics.Web.Pages.ExternalEvents
{
    public class ExternalEventsViewModel : ViewModelBase
    {
        private readonly IExternalEventInformationService _externalEventInformationService;

        public ExternalEventsViewModel(IExternalEventInformationService externalEventInformationService,
                                       EventSourcedDiagnosticsOptions options)
            : base(options)
        {
            _externalEventInformationService = externalEventInformationService;
        }

        [Bind(Direction.ServerToClient)]
        public ICollection<ExternalEventModel> ExternalEvents { get; set; } = new List<ExternalEventModel>();
        public string SelectedExternalEventName { get; set; } = string.Empty;
        public string EventData { get; set; } = "{ }";

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                ExternalEvents = _externalEventInformationService.GetAllRegisteredExternalEvents();
                SelectedExternalEventName = ExternalEvents.First()
                                                          .DisplayName;
            }
            return base.PreRender();
        }
        
        public async Task TriggerEvent()
        {
            await _externalEventInformationService.TriggerAsync(SelectedExternalEventName, EventData, RequestCancellationToken);
        }
    }
}