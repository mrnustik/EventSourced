using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.EventBus
{
    class InProcessDomainEventBus : IDomainEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public InProcessDomainEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishDomainEventAsync(IDomainEvent domainEvent, CancellationToken ct)
        {
            var domainEventType = domainEvent.GetType();
            var domainEventHandlerType = typeof(IDomainEventHandler<>)
                .MakeGenericType(domainEventType);
            var domainEventHandlers = _serviceProvider.GetServices(domainEventHandlerType);
            foreach (var domainEventHandler in domainEventHandlers)
            {
                var methodInfo = domainEventHandlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleDomainEventAsync));
                var handleTask = (Task)methodInfo!.Invoke(domainEventHandler,
                                                         new object?[]
                                                         {
                                                             domainEvent,
                                                             ct
                                                         })!;
                await handleTask;
            }
        }

        public async Task PublishDomainEventsAsync(IEnumerable<IDomainEvent> domainEvent, CancellationToken ct)
        {
            foreach (var @event in domainEvent)
            {
                await PublishDomainEventAsync(@event, ct);
            }
        }
    }
}