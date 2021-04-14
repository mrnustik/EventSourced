using EventSourced.Domain;

namespace EventSourced.Projections
{
    public abstract class AggregateProjection<TAggregateRoot, TAggregateRootId>
        where TAggregateRootId : notnull
        where TAggregateRoot : AggregateRoot<TAggregateRootId>
    {
        public TAggregateRootId Id { get; }

        protected AggregateProjection(TAggregateRootId id)
        {
            Id = id;
        }
    }
}