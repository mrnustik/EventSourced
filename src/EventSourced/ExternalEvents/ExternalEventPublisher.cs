using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventSourced.ExternalEvents
{
    class ExternalEventPublisher : IExternalEventPublisher
    {
        private readonly ExternalEventsOptions _externalEventsOptions;
        private readonly IServiceProvider _serviceProvider;

        public ExternalEventPublisher(ExternalEventsOptions externalEventsOptions, IServiceProvider serviceProvider)
        {
            _externalEventsOptions = externalEventsOptions;
            _serviceProvider = serviceProvider;
        }

        public async Task PublishExternalEventAsync(string eventType, string eventData, CancellationToken ct)
        {
            var externalEventType = _externalEventsOptions.RegisteredExternalEvents.SingleOrDefault(e => e.Name == eventType);
            if (externalEventType == null)
            {
                throw new ExternalEventNotRegisteredException($"Registration of external event of type {eventType} was not found");
            }

            var externalEventHandler = typeof(IExternalEventHandler<>);
            var handlerType = externalEventHandler.MakeGenericType(externalEventType);
            var deserializedExternalEvent = JsonConvert.DeserializeObject(eventData, externalEventType);
            
            foreach (var service in _serviceProvider.GetServices(handlerType))
            {
                var methodInfo = handlerType.GetMethod(nameof(IExternalEventHandler<object>.HandleAsync));
                await (Task) methodInfo!.Invoke(service, new [] {deserializedExternalEvent, ct})!;
            }
        }
    }
}