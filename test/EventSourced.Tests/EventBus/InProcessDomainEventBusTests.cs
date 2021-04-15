using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Domain.Events;
using EventSourced.EventBus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EventSourced.Tests.EventBus
{
    public class InProcessDomainEventBusTests
    {
        [Fact]
        public async Task PublishDomainEventAsync_WithRegisteredEventHandlers_CallsThem()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();
            var firstTestEventHandlerMock = new Mock<IDomainEventHandler<TestEvent>>();
            var secondTestEventHandlerMock = new Mock<IDomainEventHandler<TestEvent>>();
            var otherTestEventHandlerMock = new Mock<IDomainEventHandler<OtherTestEvent>>();
            serviceCollection.AddSingleton(firstTestEventHandlerMock.Object);
            serviceCollection.AddSingleton(secondTestEventHandlerMock.Object);
            serviceCollection.AddSingleton(otherTestEventHandlerMock.Object);
            var sut = CreateSut(serviceCollection);
            
            //Act
            await sut.PublishDomainEventAsync(new TestEvent(), CancellationToken.None);

            //Assert
            VerifyDomainHandlerCalled(firstTestEventHandlerMock);
            VerifyDomainHandlerCalled(secondTestEventHandlerMock);
            VerifyDomainHandlerNotCalled(otherTestEventHandlerMock);
        }
        
        [Fact]
        public async Task PublishDomainEventAsync_WithoutRegisteredEventHandlers_DoesNotThrow()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();
            var sut = CreateSut(serviceCollection);
            
            //Act
            Func<Task> publishTask = () => sut.PublishDomainEventAsync(new TestEvent(), CancellationToken.None);

            //Assert
            await publishTask.Should()
                      .NotThrowAsync();
        }
        
        [Fact]
        public async Task PublishDomainEventsAsync_WithRegisteredEventHandlers_CallsThem()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();
            var firstTestEventHandlerMock = new Mock<IDomainEventHandler<TestEvent>>();
            var secondTestEventHandlerMock = new Mock<IDomainEventHandler<TestEvent>>();
            var otherTestEventHandlerMock = new Mock<IDomainEventHandler<OtherTestEvent>>();
            serviceCollection.AddSingleton(firstTestEventHandlerMock.Object);
            serviceCollection.AddSingleton(secondTestEventHandlerMock.Object);
            serviceCollection.AddSingleton(otherTestEventHandlerMock.Object);
            var sut = CreateSut(serviceCollection);
            
            //Act
            await sut.PublishDomainEventsAsync(new DomainEvent[] {new TestEvent(), new OtherTestEvent()}, CancellationToken.None);

            //Assert
            VerifyDomainHandlerCalled(firstTestEventHandlerMock);
            VerifyDomainHandlerCalled(secondTestEventHandlerMock);
            VerifyDomainHandlerCalled(otherTestEventHandlerMock);
        }

        private void VerifyDomainHandlerCalled<TDomainEvent>(Mock<IDomainEventHandler<TDomainEvent>> mock) where TDomainEvent : IDomainEvent
        {
            mock.Verify(t => t.HandleDomainEventAsync(It.IsAny<TDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void VerifyDomainHandlerNotCalled<TDomainEvent>(Mock<IDomainEventHandler<TDomainEvent>> mock) where TDomainEvent : IDomainEvent
        {
            mock.Verify(t => t.HandleDomainEventAsync(It.IsAny<TDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        
        private IDomainEventBus CreateSut(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return new InProcessDomainEventBus(serviceProvider);
        }

        public class TestEvent : DomainEvent
        {
        }
        
        public class OtherTestEvent : DomainEvent
        {
        }
    }
}