using System;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Helpers;

namespace EventSourced.Diagnostics.Web.Model.Projections
{
    public class TypeBasedProjectionModel
    {
        [Bind(Direction.None)]
        public Type ProjectionType { get; } 
        public string AssemblyQualifiedName { get; }
        public string DisplayName { get; }
        public string EncodedTypeId { get; }
        public string SerializedProjection { get; }

        public TypeBasedProjectionModel(Type projectionType, string serializedProjection)
        {
            ProjectionType = projectionType;
            SerializedProjection = serializedProjection;
            AssemblyQualifiedName = projectionType.AssemblyQualifiedName!;
            DisplayName = projectionType.Name;
            EncodedTypeId = TypeEncoder.EncodeType(projectionType);
        }
    }
}