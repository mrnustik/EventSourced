﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventSourced.Abstractions.Domain.Events;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Domain
{
    public abstract class AggregateRoot
    {
        private readonly Queue<IDomainEvent> uncommittedDomainEvents;

        protected AggregateRoot()
        {
            uncommittedDomainEvents = new Queue<IDomainEvent>();
        }

        public IList<IDomainEvent> DequeueDomainEvents()
        {
            return uncommittedDomainEvents.DequeueAll();
        }

        protected void EnqueueAndApplyEvent(IDomainEvent domainEvent)
        {
            uncommittedDomainEvents.Enqueue(domainEvent);
            TryToApplyEvent(domainEvent);
        }

        private void TryToApplyEvent(IDomainEvent domainEvent)
        {
            var applyMethod = ReflectionHelpers.GetApplyMethodForEventInAggregateRoot(this, domainEvent);
            if (applyMethod != null)
            {
                applyMethod.Invoke(this, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public , null , new[] {domainEvent}, null);
            }
            else
            {
                throw new ArgumentException(
                    $"Missing Apply event for domain event of type {domainEvent.GetType()} on aggregate {GetType()}");
            }
        }
    }
}