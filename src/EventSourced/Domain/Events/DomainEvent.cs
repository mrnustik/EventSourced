namespace EventSourced.Domain.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public int Version { get; set; }
    }
}