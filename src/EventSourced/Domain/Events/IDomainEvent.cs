using System;

namespace EventSourced.Domain.Events
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        int Version { get; set; }
    }
}