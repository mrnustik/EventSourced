using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Helpers;
using EventSourced.Diagnostics.Web.Model.Projections;
using EventSourced.Diagnostics.Web.Services;

namespace EventSourced.Diagnostics.Web.Pages.ProjectionsList
{
    public class ProjectionsListViewModel : ViewModelBase
    {
        private readonly IProjectionInformationService _projectionInformationService;

        public ProjectionsListViewModel(IProjectionInformationService projectionInformationService)
        {
            _projectionInformationService = projectionInformationService;
        }

        public ICollection<TypeBasedProjectionModel> TypeBasedProjections { get; set; } = new List<TypeBasedProjectionModel>();
        public TypeBasedProjectionModel? SelectedProjectionModel { get; set; }
        public string? SelectedProjectionEncodedType { get; set; }

        public override async Task PreRender()
        {
            await base.PreRender();
            TypeBasedProjections = await _projectionInformationService.GetTypeBasedProjectionsAsync(RequestCancellationToken);
            SelectedProjectionModel = TypeBasedProjections.FirstOrDefault();
            SelectedProjectionEncodedType = SelectedProjectionModel?.EncodedTypeId;
        }

        public void OnProjectionChanged()
        {
            if (SelectedProjectionEncodedType == null) return;
            var decodedType = TypeEncoder.DecodeType(SelectedProjectionEncodedType);
            SelectedProjectionModel = TypeBasedProjections.SingleOrDefault(p => p.ProjectionType == decodedType);
        }
    }
}