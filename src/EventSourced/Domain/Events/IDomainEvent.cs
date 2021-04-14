namespace EventSourced.Domain.Events
{
    public interface IDomainEvent
    {
        int Version { get; set; }
    }
}