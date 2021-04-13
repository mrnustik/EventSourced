using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Abstractions.Domain.Events;
using EventSourced.Persistence.Abstractions;

namespace EventSourced.Persistence.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private ConcurrentDictionary<StreamIdentification, List<IDomainEvent>> StreamsDictionary { get; }

        public InMemoryEventStore()
            : this(new Dictionary<StreamIdentification, List<IDomainEvent>>())
        {
        }
        
        public InMemoryEventStore(IDictionary<StreamIdentification, List<IDomainEvent>> originalState)
        {
            StreamsDictionary = new ConcurrentDictionary<StreamIdentification, List<IDomainEvent>>(originalState);
        }

        public Task StoreEventsAsync(string streamId, Type aggregateRootType, IList<IDomainEvent> domainEvents, CancellationToken ct)
        {
            var streamIdentification = new StreamIdentification(streamId, aggregateRootType);
            StreamsDictionary.AddOrUpdate(streamIdentification,
                _ => domainEvents.ToList(),
                (_, existingEvents) => existingEvents.Concat(domainEvents).ToList());
            return Task.CompletedTask;
        }

        public Task<IDomainEvent[]> GetByStreamIdAsync(string streamId, Type aggregateRootType, CancellationToken ct)
        {
            var streamIdentification = new StreamIdentification(streamId, aggregateRootType);
            
            if (StreamsDictionary.TryGetValue(streamIdentification, out var events))
            {
                var eventsArray = events.ToArray();
                return Task.FromResult(eventsArray);
            }

            throw new NotImplementedException();
        }
    }
    
    public record StreamIdentification(string StreamId, Type AggregateRootType);
}