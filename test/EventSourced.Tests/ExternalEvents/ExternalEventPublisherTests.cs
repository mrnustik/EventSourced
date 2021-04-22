using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Exceptions;
using EventSourced.ExternalEvents;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EventSourced.Tests.ExternalEvents
{
    public class ExternalEventPublisherTests
    {
        [Fact]
        public async Task PublishExternalEventAsync_WithRegisteredHandlers_Calls_It()
        {
            //Arrange
            var handlerMock = new Mock<IExternalEventHandler<TestExternalEvent>>();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(handlerMock.Object);
            var options = new ExternalEventsOptions();
            options.RegisteredExternalEvents.Add(typeof(TestExternalEvent));
            var sut = CreateSut(options, serviceCollection.BuildServiceProvider());

            //Act
            await sut.PublishExternalEventAsync(nameof(TestExternalEvent), "{ \"Number\": 42}", CancellationToken.None);

            //Assert
            handlerMock.Verify(s => s.HandleAsync(It.IsAny<TestExternalEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task PublishExternalEventAsync_WithoutRegisteredHandlers_Throws()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();
            var options = new ExternalEventsOptions();
            var sut = CreateSut(options, serviceCollection.BuildServiceProvider());

            //Act
            Func<Task> action = () => sut.PublishExternalEventAsync(nameof(TestExternalEvent), "{ \"Number\": 42}", CancellationToken.None);

            //Assert
            await action.Should()
                        .ThrowAsync<ExternalEventNotRegisteredException>();
        }
        
        private IExternalEventPublisher CreateSut(ExternalEventsOptions options, IServiceProvider serviceProvider)
        {
            return new ExternalEventPublisher(options, serviceProvider);
        }

        public class TestExternalEvent
        {
            public int Number { get; set; }
        }
    }
}