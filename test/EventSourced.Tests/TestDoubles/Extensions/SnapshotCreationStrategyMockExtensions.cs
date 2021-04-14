using EventSourced.Domain;
using EventSourced.Snapshots;
using Moq;

namespace EventSourced.Tests.TestDoubles.Extensions
{
    public static class SnapshotCreationStrategyMockExtensions
    {
        public static Mock<ISnapshotCreationStrategy> WithShouldCreateSnapshot<TAggregateRoot>(this Mock<ISnapshotCreationStrategy> mock, bool result) 
            where TAggregateRoot : AggregateRoot
        {
            mock.Setup(s => s.ShouldCreateSnapshot(It.IsAny<TAggregateRoot>()))
                .Returns(result);
            return mock;
        }
    }
}