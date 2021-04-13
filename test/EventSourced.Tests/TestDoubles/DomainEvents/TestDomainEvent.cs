using EventSourced.Domain.Events;

namespace EventSourced.Tests.TestDoubles.DomainEvents
{
    public class TestDomainEvent : DomainEvent
    {
        public TestDomainEvent(string parameter)
        {
            Parameter = parameter;
        }

        public string Parameter { get; }
    }
}