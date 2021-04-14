using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EventSourced.Domain.Events;
using EventSourced.Persistence;
using Moq;

namespace EventSourced.Tests.TestDoubles.Extensions
{
    public static class EventStoreMockExtensions
    {
        public static Mock<IEventStore> WithStreamExistsAsync(this Mock<IEventStore> mock, bool result)
        {
            mock.Setup(s => s.StreamExistsAsync(It.IsAny<Guid>(), It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            return mock;
        }

        public static Mock<IEventStore> WithGetByStreamIdAsync(this Mock<IEventStore> mock,
                                                               Guid streamId,
                                                               IEnumerable<IDomainEvent> domainEvents)
        {
            mock.Setup(s => s.GetByStreamIdAsync(streamId, It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(domainEvents.ToArray());
            return mock;
        }

        public static Mock<IEventStore> WithGetAllStreamsOfType(this Mock<IEventStore> mock,
                                                                IDictionary<Guid, IDomainEvent[]> aggregateToEventsMap)
        {
            mock.Setup(s => s.GetAllStreamsOfType(It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aggregateToEventsMap);
            return mock;
        }
    }
}