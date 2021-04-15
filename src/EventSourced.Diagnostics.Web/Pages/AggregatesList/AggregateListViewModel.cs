using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Helpers;
using EventSourced.Diagnostics.Web.Model;
using EventSourced.Diagnostics.Web.Services;

namespace EventSourced.Diagnostics.Web.Pages.AggregatesList
{
    public class AggregatesListViewModel : ViewModelBase
    {
        private readonly IAggregateInformationService _aggregateInformationService;

        public AggregatesListViewModel(IAggregateInformationService aggregateInformationService)
        {
            _aggregateInformationService = aggregateInformationService;
        }

        [Bind(Direction.None)]
        public Type AggregateType => TypeSerializer.DeserializeType(Base64Encoder.Decode(EncodedAggregateTypeId));
        [FromRoute("AggregateType")]
        public string EncodedAggregateTypeId { get; set; } = null!;

        public ICollection<AggregateInstancesListItemModel> AggregateInstances { get; set; } = new List<AggregateInstancesListItemModel>();

        public override async Task PreRender()
        {
            await base.PreRender();
            AggregateInstances = await _aggregateInformationService.GetStoredAggregatesOfType(AggregateType, RequestCancellationToken);
        }
    }
}