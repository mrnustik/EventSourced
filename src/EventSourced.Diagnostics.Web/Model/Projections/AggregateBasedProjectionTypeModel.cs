using System;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Helpers;

namespace EventSourced.Diagnostics.Web.Model.Projections
{
    public class AggregateBasedProjectionTypeModel
    {
        [Bind(Direction.None)]
        public Type ProjectionType { get; }
        public string AssemblyQualifiedName { get; }
        public string DisplayName { get; }
        public string EncodedTypeId { get; }

        public AggregateBasedProjectionTypeModel(Type projectionType)
        {
            ProjectionType = projectionType;
            AssemblyQualifiedName = projectionType.AssemblyQualifiedName!;
            DisplayName = projectionType.Name;
            EncodedTypeId = TypeEncoder.EncodeType(projectionType);
        }
    }
}