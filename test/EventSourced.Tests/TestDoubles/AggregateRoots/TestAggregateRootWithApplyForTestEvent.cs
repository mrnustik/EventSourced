using System;
using EventSourced.Domain;
using EventSourced.Tests.TestDoubles.DomainEvents;

namespace EventSourced.Tests.TestDoubles.AggregateRoots
{
    public class TestAggregateRootWithApplyForTestEvent : AggregateRoot<Guid>
    {
        public string ParameterValue { get; private set; } = string.Empty;

        public TestAggregateRootWithApplyForTestEvent(Guid id) : base(id)
        {
        }
        
        public void EnqueueTestDomainEvent(string parameter)
        {
            var domainEvent = new TestDomainEvent(parameter);
            EnqueueAndApplyEvent(domainEvent);
        }

        private void Apply(TestDomainEvent domainEvent)
        {
            ParameterValue = domainEvent.Parameter;
        }
    }
}