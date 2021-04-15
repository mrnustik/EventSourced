namespace EventSourced.Domain.Events
{
    public abstract class DomainEvent
    {
        public abstract int Version { get; internal set; }
    }
}