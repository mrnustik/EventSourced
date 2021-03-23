using EventSourced.Domain;
using EventSourced.Tests.TestDoubles.DomainEvents;

namespace EventSourced.Tests.TestDoubles.AggregateRoots
{
    public class TestAggregateRootWithApplyForTestEvent : AggregateRoot
    {
        public string ParameterValue { get; private set; } = string.Empty;
        
        public void EnqueueTestDomainEvent(string parameter)
        {
            var domainEvent = new TestDomainEvent(parameter);
            EnqueueAndApplyEvent(domainEvent);
        }

        public void Apply(TestDomainEvent domainEvent)
        {
            ParameterValue = domainEvent.Parameter;
        }
    }
}