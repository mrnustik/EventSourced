﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Helpers;
using EventSourced.Persistence.Abstractions;

namespace EventSourced.Persistence
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        
        public Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            var newDomainEvents = aggregateRoot.DequeueDomainEvents();
            return _eventStore.StoreEventsAsync(newDomainEvents, ct);
        }

        public async Task<TAggregateRoot> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var domainEvents = await _eventStore.GetByStreamIdAsync(id, ct);
            var aggregateRoot = new TAggregateRoot();
            foreach (var domainEvent in domainEvents)
            {
                var applyMethod = ReflectionHelpers.GetApplyMethodForEventInAggregateRoot(aggregateRoot, domainEvent);
                if (applyMethod != null)
                {
                    applyMethod.Invoke(aggregateRoot, new[] {domainEvent});
                }
                else
                {
                    throw new ArgumentException(
                        $"Missing Apply event for domain event of type {domainEvent.GetType()} on aggregate {aggregateRoot.GetType()}");
                }
            }
            return aggregateRoot;
        }
    }
}