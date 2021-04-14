using System;
using EventSourced.Domain;

namespace EventSourced.Projections
{
    public abstract class AggregateProjection<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        public Guid Id { get; }

        protected AggregateProjection(Guid id)
        {
            Id = id;
        }
    }
}