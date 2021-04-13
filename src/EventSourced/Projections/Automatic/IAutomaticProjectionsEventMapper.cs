using System;
using System.Collections.Generic;

namespace EventSourced.Projections.Automatic
{
    public interface IAutomaticProjectionsEventMapper
    {
        void Initialize();
        IEnumerable<Type> GetProjectionsAffectedByEvent(Type eventType);
    }
}