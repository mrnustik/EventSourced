namespace EventSourced.Domain.Events
{
    public abstract class DomainEvent
    {
        public int Version { get; internal set; }
    }
}