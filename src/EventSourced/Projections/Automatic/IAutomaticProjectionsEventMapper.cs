using System;
using System.Collections.Generic;
using EventSourced.Domain.Events;

namespace EventSourced.Projections.Automatic
{
    public interface IAutomaticProjectionsEventMapper
    {
        void Initialize();
        IEnumerable<Type> GetProjectionsAffectedByEvent(IDomainEvent domainEvent);
    }
}