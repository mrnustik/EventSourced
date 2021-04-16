﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Model;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Helpers;
using EventSourced.Persistence;
using Newtonsoft.Json;

namespace EventSourced.Diagnostics.Web.Services
{
    public class AggregateInformationService : IAggregateInformationService
    {
        private readonly IEventStore _eventStore;

        public AggregateInformationService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<ICollection<AggregateTypesListItemModel>> GetStoredAggregateTypesAsync(CancellationToken ct)
        {
            var types = await _eventStore.GetAllAggregateTypes(ct);
            return types.Select(t => new AggregateTypesListItemModel(t))
                        .ToList();
        }

        public async Task<ICollection<AggregateInstancesListItemModel>> GetStoredAggregatesOfTypeAsync(Type aggregateType, CancellationToken ct)
        {
            var streams = await _eventStore.GetAllStreamsOfType(aggregateType, ct);
            var aggregates = RebuildAggregatesFromStreams(streams, aggregateType);
            return aggregates.Select(i => new AggregateInstancesListItemModel(i.Id, i.Version, JsonConvert.SerializeObject(i)))
                             .ToList();
        }

        public async Task<AggregateInstancesListItemModel> GetStoredAggregateByIdAndVersionAsync(Guid aggregateId, Type aggregateType, int version, CancellationToken ct)
        {
            var stream = await _eventStore.GetByStreamIdAsync(aggregateId, aggregateType, 0, ct);
            var versionedStream = stream.Where(e => e.Version <= version)
                                     .ToArray();
            var aggregate = RebuildAggregateFromStream(aggregateType, aggregateId, versionedStream);
            return new AggregateInstancesListItemModel(aggregate.Id, aggregate.Version, JsonConvert.SerializeObject(aggregate));
        }

        private IEnumerable<AggregateRoot> RebuildAggregatesFromStreams(IDictionary<Guid, IDomainEvent[]> streams, Type aggregateType)
        {
            foreach (var (streamId, events) in streams)
            {
                yield return RebuildAggregateFromStream(aggregateType, streamId, events);
            }
        }

        private static AggregateRoot RebuildAggregateFromStream(Type aggregateType, Guid streamId, IDomainEvent[] events)
        {
            var aggregateRoot = AggregateRootFactory.CreateAggregateRoot(streamId, aggregateType);
            aggregateRoot.RebuildFromEvents(events);
            return aggregateRoot;
        }
    }
}