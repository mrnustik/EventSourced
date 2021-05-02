using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Configuration;
using EventSourced.Diagnostics.Web.Model.Aggregates;
using EventSourced.Diagnostics.Web.Services;

namespace EventSourced.Diagnostics.Web.Pages.AggregateTypes
{
    public class AggregateTypesListViewModel : ViewModelBase
    {
        private readonly IAggregateInformationService _aggregateInformationService;

        public AggregateTypesListViewModel(IAggregateInformationService aggregateInformationService,
                                           EventSourcedDiagnosticsOptions options)
            : base(options)
        {
            _aggregateInformationService = aggregateInformationService;
        }

        [Bind(Direction.ServerToClient)]
        public ICollection<AggregateTypesListItemModel> AggregateTypes { get; set; } = new List<AggregateTypesListItemModel>();
        
        public override async Task PreRender()
        {
            await base.PreRender();
            AggregateTypes = await _aggregateInformationService.GetStoredAggregateTypesAsync(RequestCancellationToken);
        }
    }
}