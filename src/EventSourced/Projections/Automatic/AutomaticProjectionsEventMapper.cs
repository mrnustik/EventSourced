using System;
using System.Collections.Generic;
using System.Linq;
using EventSourced.Configuration;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Projections.Automatic
{
    internal class AutomaticProjectionsEventMapper : IAutomaticProjectionsEventMapper
    {
        private readonly AutomaticProjectionOptions _options;

        private bool _isInitialized;

        private Dictionary<Type, ICollection<Type>> EventsToProjectionMap { get; } = new();
        private Dictionary<Type, ICollection<Type>> AggregateToAggregateProjectionMap { get; } = new();

        public AutomaticProjectionsEventMapper(AutomaticProjectionOptions options)
        {
            _options = options;
        }

        public void Initialize()
        {
            EnsureNotAlreadyInitialized();
            InitializeAutomaticProjectionsMap();
            InitializeAutomaticAggregateProjectionMap();
            _isInitialized = true;
        }

        public IEnumerable<Type> GetProjectionsAffectedByEvent(IDomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();
            if (EventsToProjectionMap.TryGetValue(eventType, out var eventsCollection))
            {
                return eventsCollection.ToList()
                                       .AsReadOnly();
            }
            return Enumerable.Empty<Type>();
        }

        public IEnumerable<Type> GetProjectionsAffectedByAggregateChange(Type aggregateType)
        {
            if (AggregateToAggregateProjectionMap.TryGetValue(aggregateType, out var eventsCollection))
            {
                return eventsCollection.ToList()
                                       .AsReadOnly();
            }
            return Enumerable.Empty<Type>();
        }

        private void InitializeAutomaticAggregateProjectionMap()
        {
            foreach (var aggregateProjectionType in _options.RegisteredAutomaticAggregateProjections)
            {
                var aggregateRootType = ReflectionHelpers.GetAggregateRootTypeFromProjection(aggregateProjectionType);
                AddOrUpdateAggregateProjectionInAggregateMap(aggregateRootType, aggregateProjectionType);
            }
        }

        private void InitializeAutomaticProjectionsMap()
        {
            foreach (var projectionType in _options.RegisteredAutomaticProjections)
            {
                var applicableEvents = ReflectionHelpers.GetTypesOfDomainEventsApplicableToObject(projectionType)
                                                        .ToList();
                if (!applicableEvents.Any())
                {
                    throw new ArgumentException($"Projection of type {projectionType.Name} has no applicable event.");
                }

                foreach (var applicableEvent in applicableEvents)
                {
                    AddOrUpdateProjectionInEventMap(applicableEvent, projectionType);
                }
            }
        }

        private void EnsureNotAlreadyInitialized()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("AutomaticProjectionEventMapper is already initialized");
            }
        }

        private void AddOrUpdateProjectionInEventMap(Type eventType, Type projectionType)
        {
            if (EventsToProjectionMap.ContainsKey(eventType))
            {
                EventsToProjectionMap[eventType]
                    .Add(projectionType);
            }
            else
            {
                EventsToProjectionMap[eventType] = new List<Type> {projectionType};
            }
        }

        private void AddOrUpdateAggregateProjectionInAggregateMap(Type aggregateType, Type projectionType)
        {
            if (AggregateToAggregateProjectionMap.ContainsKey(aggregateType))
            {
                AggregateToAggregateProjectionMap[aggregateType]
                    .Add(projectionType);
            }
            else
            {
                AggregateToAggregateProjectionMap[aggregateType] = new List<Type> {projectionType};
            }
        }
    }
}