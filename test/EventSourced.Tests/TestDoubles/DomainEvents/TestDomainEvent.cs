using EventSourced.Domain.Events;

namespace EventSourced.Tests.TestDoubles.DomainEvents
{
    public class TestDomainEvent : DomainEvent
    {
        public string Parameter { get; }

        public TestDomainEvent(string parameter)
        {
            Parameter = parameter;
        }
    }
}