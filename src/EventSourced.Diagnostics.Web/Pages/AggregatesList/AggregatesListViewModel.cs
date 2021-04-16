using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Helpers;
using EventSourced.Diagnostics.Web.Model.Aggregates;
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
        public string? AggregateDisplayName => AggregateType.Name;
        public string? AggregateFullName => AggregateType.FullName;
        [Bind(Direction.ServerToClientFirstRequest)]
        public ICollection<AggregateInstancesListItemModel> AggregateInstances { get; set; } =
            new List<AggregateInstancesListItemModel>();
        [Bind(Direction.ServerToClient)]
        public AggregateInstancesListItemModel? SelectedAggregateInstance { get; set; }
        public Guid? SelectedAggregateInstanceId { get; set; }
        public int? SelectedVersion { get; set; }
        public int? SelectedMaxVersion { get; set; }

        public override async Task Load()
        {
            await base.Load();
            if (!Context.IsPostBack)
            {
                AggregateInstances =
                    await _aggregateInformationService.GetStoredAggregatesOfTypeAsync(AggregateType, RequestCancellationToken);
                SelectedAggregateInstance = AggregateInstances.FirstOrDefault();
                SelectedAggregateInstanceId = SelectedAggregateInstance?.Id;
                SelectedVersion = SelectedAggregateInstance?.Version;
                SelectedMaxVersion = SelectedVersion;
            }
        }

        public async Task OnAggregateInstanceChanged()
        {
            AggregateInstances =
                await _aggregateInformationService.GetStoredAggregatesOfTypeAsync(AggregateType, RequestCancellationToken);
            SelectedAggregateInstance = AggregateInstances.SingleOrDefault(i => i.Id == SelectedAggregateInstanceId);
            SelectedVersion = SelectedAggregateInstance?.Version;
            SelectedMaxVersion = SelectedVersion;
        }

        public async Task ChangeVersion(int version)
        {
            if (!SelectedAggregateInstanceId.HasValue) return;
            SelectedAggregateInstance =
                await _aggregateInformationService.GetStoredAggregateByIdAndVersionAsync(
                    SelectedAggregateInstanceId.Value,
                    AggregateType,
                    version,
                    RequestCancellationToken);
            SelectedVersion = SelectedAggregateInstance.Version;
        }
    }
}