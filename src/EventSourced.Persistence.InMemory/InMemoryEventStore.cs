using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;

namespace EventSourced.Persistence.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        public InMemoryEventStore()
            : this(new Dictionary<StreamIdentification, List<IDomainEvent>>())
        {
        }

        public InMemoryEventStore(IDictionary<StreamIdentification, List<IDomainEvent>> originalState)
        {
            StreamsDictionary = new ConcurrentDictionary<StreamIdentification, List<IDomainEvent>>(originalState);
        }

        private ConcurrentDictionary<StreamIdentification, List<IDomainEvent>> StreamsDictionary { get; }

        public Task StoreEventsAsync(Guid streamId, Type aggregateRootType, IList<IDomainEvent> domainEvents, CancellationToken ct)
        {
            var streamIdentification = new StreamIdentification(streamId, aggregateRootType);
            StreamsDictionary.AddOrUpdate(streamIdentification,
                _ => domainEvents.ToList(),
                (_, existingEvents) => existingEvents.Concat(domainEvents).ToList());
            return Task.CompletedTask;
        }

        public Task<IDomainEvent[]> GetByStreamIdAsync(Guid streamId, Type aggregateRootType, CancellationToken ct)
        {
            var streamIdentification = new StreamIdentification(streamId, aggregateRootType);

            if (StreamsDictionary.TryGetValue(streamIdentification, out var events))
            {
                var eventsArray = events.ToArray();
                return Task.FromResult(eventsArray);
            }

            throw new NotImplementedException();
        }

        public Task<bool> StreamExistsAsync(Guid streamId, Type aggregateRootType, CancellationToken ct)
        {
            var streamExists = StreamsDictionary.ContainsKey(new StreamIdentification(streamId, aggregateRootType));
            return Task.FromResult(streamExists);
        }

        public Task<IDictionary<Guid, IDomainEvent[]>> GetAllStreamsOfType(Type aggregateRootType, CancellationToken ct)
        {
            IDictionary<Guid, IDomainEvent[]> allStreams =
                StreamsDictionary.Where(d => d.Key.AggregateRootType == aggregateRootType)
                    .Select(d => new {d.Key.StreamId, DomainEvents = d.Value.ToArray()})
                    .ToDictionary(d => d.StreamId, d => d.DomainEvents);
            return Task.FromResult(allStreams);
        }

        public Task<IDomainEvent[]> GetEventsOfTypeAsync(Type eventType, CancellationToken ct)
        {
            var events = StreamsDictionary.Values
                .SelectMany(v => v)
                .Where(v => v.GetType() == eventType)
                .ToArray();
            return Task.FromResult(events);
        }
    }

    public record StreamIdentification(Guid StreamId, Type AggregateRootType);
}