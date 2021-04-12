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
        private ConcurrentDictionary<string, List<IDomainEvent>> StreamsDictionary { get; }

        public InMemoryEventStore()
            : this(new Dictionary<string, List<IDomainEvent>>())
        {
        }
        
        public InMemoryEventStore(IDictionary<string, List<IDomainEvent>> originalState)
        {
            StreamsDictionary = new ConcurrentDictionary<string, List<IDomainEvent>>(originalState);
        }

        public Task StoreEventsAsync(string streamId, IList<IDomainEvent> domainEvents, CancellationToken ct)
        {
            StreamsDictionary.AddOrUpdate(streamId,
                _ => domainEvents.ToList(),
                (_, existingEvents) => existingEvents.Concat(domainEvents).ToList());
            return Task.CompletedTask;
        }

        public Task<IDomainEvent[]> GetByStreamIdAsync(string streamId, CancellationToken ct)
        {
            if (StreamsDictionary.TryGetValue(streamId, out var events))
            {
                var eventsArray = events.ToArray();
                return Task.FromResult(eventsArray);
            }

            throw new NotImplementedException();
        }
    }
}