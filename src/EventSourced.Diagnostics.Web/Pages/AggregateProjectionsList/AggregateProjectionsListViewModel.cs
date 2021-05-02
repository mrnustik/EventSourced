using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Configuration;
using EventSourced.Diagnostics.Web.Model.Projections;
using EventSourced.Diagnostics.Web.Services;

namespace EventSourced.Diagnostics.Web.Pages.AggregateProjectionsList
{
    public class AggregateProjectionsListViewModel : ViewModelBase
    {
        private readonly IProjectionInformationService _projectionInformationService;

        public AggregateProjectionsListViewModel(IProjectionInformationService projectionInformationService,
                                                 EventSourcedDiagnosticsOptions options)
            : base(options)
        {
            _projectionInformationService = projectionInformationService;
        }

        public ICollection<AggregateBasedProjectionTypeModel> AggregateProjectionTypes { get; set; } =
            new List<AggregateBasedProjectionTypeModel>();

        public override async Task PreRender()
        {
            await base.PreRender();
            AggregateProjectionTypes =
                await _projectionInformationService.GetAllAggregateProjectionTypesAsync(RequestCancellationToken);
        }
    }
}