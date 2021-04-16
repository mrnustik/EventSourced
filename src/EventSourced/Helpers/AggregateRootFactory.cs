using System;
using EventSourced.Domain;

namespace EventSourced.Helpers
{
    public class AggregateRootFactory
    {
        public static TAggregateRoot CreateAggregateRoot<TAggregateRoot>(Guid id)
            where TAggregateRoot : AggregateRoot
            => (TAggregateRoot) CreateAggregateRoot(id, typeof(TAggregateRoot));
        
        public static AggregateRoot CreateAggregateRoot(Guid id, Type type)
        {
            var aggregateRoot = (AggregateRoot) Activator.CreateInstance(type, id)!;
            return aggregateRoot;
        }
    }
}