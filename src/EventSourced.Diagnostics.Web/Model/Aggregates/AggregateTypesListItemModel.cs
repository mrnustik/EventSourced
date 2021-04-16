using System;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Helpers;

namespace EventSourced.Diagnostics.Web.Model.Aggregates
{
    public class AggregateTypesListItemModel
    {
        [Bind(Direction.None)]
        public Type AggregateType { get; } 
        public string AssemblyQualifiedName { get; }
        public string DisplayName { get; }
        public string EncodedTypeId { get; }

        public AggregateTypesListItemModel(Type aggregateType)
        {
            AggregateType = aggregateType;
            AssemblyQualifiedName = aggregateType.AssemblyQualifiedName!;
            DisplayName = aggregateType.Name;
            EncodedTypeId = TypeEncoder.EncodeType(aggregateType);
        }
    }
}