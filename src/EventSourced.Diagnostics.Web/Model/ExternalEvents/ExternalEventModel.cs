using System;
using DotVVM.Framework.ViewModel;
using EventSourced.Diagnostics.Web.Helpers;

namespace EventSourced.Diagnostics.Web.Model.ExternalEvents
{
    public class ExternalEventModel
    {
        [Bind(Direction.None)]
        public Type ExternalEventType { get; }
        public string AssemblyQualifiedName { get; }
        public string DisplayName { get; }
        public string EncodedTypeId { get; }

        public ExternalEventModel(Type externalEventType)
        {
            ExternalEventType = externalEventType;
            AssemblyQualifiedName = externalEventType.AssemblyQualifiedName!;
            DisplayName = externalEventType.Name;
            EncodedTypeId = TypeEncoder.EncodeType(externalEventType);
        }
    }
    
}