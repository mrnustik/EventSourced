using System;

namespace EventSourced.Abstractions.Domain.Events
{
    public interface IDomainEvent
    {
        Guid Id { get; }
    }
}