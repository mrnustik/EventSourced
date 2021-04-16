using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
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
        
        [Bind(Direction.ServerToClient)]
        public ICollection<TypeBasedProjectionModel> TypeBasedProjections { get; set; } = new List<TypeBasedProjectionModel>();
        [Bind(Direction.ServerToClient)]
        public TypeBasedProjectionModel? SelectedProjectionModel { get; set; }
        public string? SelectedProjectionEncodedType { get; set; }

        public override async Task PreRender()
        {
            await base.PreRender();
            if (!Context.IsPostBack)
            {
                TypeBasedProjections = await _projectionInformationService.GetTypeBasedProjectionsAsync(RequestCancellationToken);
                SelectedProjectionModel = TypeBasedProjections.FirstOrDefault();
                SelectedProjectionEncodedType = SelectedProjectionModel?.EncodedTypeId;
            }
        }

        public async Task OnProjectionChanged()
        {
            if (SelectedProjectionEncodedType == null) return;
            TypeBasedProjections = await _projectionInformationService.GetTypeBasedProjectionsAsync(RequestCancellationToken);
            var decodedType = TypeEncoder.DecodeType(SelectedProjectionEncodedType);
            SelectedProjectionModel = TypeBasedProjections.SingleOrDefault(p => p.ProjectionType == decodedType);
            SelectedProjectionEncodedType = SelectedProjectionModel?.EncodedTypeId;
        }
    }
}