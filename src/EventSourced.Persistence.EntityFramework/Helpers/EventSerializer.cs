using System;
using EventSourced.Domain.Events;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace EventSourced.Persistence.EntityFramework.Helpers
{
    public class EventSerializer : IEventSerializer
    {
        private JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new PrivateSetterAndCtorContractResolver()
        };

        public string SerializeEvent(IDomainEvent domainEvent)
        {
            return JsonConvert.SerializeObject(domainEvent, SerializerSettings);
        }

        public IDomainEvent DeserializeEvent(string serializedEvent, Type eventType)
        {
            return (IDomainEvent) JsonConvert.DeserializeObject(serializedEvent, eventType, SerializerSettings)!;
        }
    }
}