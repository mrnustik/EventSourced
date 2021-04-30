namespace EventSourced.Domain.Events
{
    public interface IApplyDomainEvent<in TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        void Apply(TDomainEvent domainEvent);
    }
}