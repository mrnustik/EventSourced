using System;

namespace EventSourced.Domain.Events
{
    public interface IDomainEvent
    {
        Guid Id { get; }
    }
}