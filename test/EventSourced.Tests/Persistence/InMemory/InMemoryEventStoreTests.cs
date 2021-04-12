﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Abstractions.Domain.Events;
using EventSourced.Domain.Events;
using EventSourced.Persistence.InMemory;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.InMemory
{
    public class InMemoryEventStoreTests
    {
        [Fact]
        public async Task StoreEventsAsync_WithNonExistingAggregate_PreserversOrder()
        {
            //Arrange
            var sut = CreateSut();
            var streamId = Guid.NewGuid();
            var testEvents = new[]
            {
                new TestEvent(1),
                new TestEvent(2),
                new TestEvent(3)
            };
            
            //Act
            await sut.StoreEventsAsync(streamId.ToString(), testEvents.Cast<IDomainEvent>().ToList(), CancellationToken.None);

            //Assert
            var events = await sut.GetByStreamIdAsync(streamId.ToString(), CancellationToken.None);
            events.Should()
                .HaveCount(3)
                .And
                .AllBeOfType<TestEvent>()
                .And
                .Subject
                .Cast<TestEvent>()
                .Should()
                .BeInAscendingOrder(e => e.Number);
        }
        
        [Fact]
        public async Task StoreEventsAsync_WithExistingAggregate_PreserversOrder()
        {
            //Arrange
            var streamId = Guid.NewGuid();
            var sut = CreateSut(new Dictionary<string, List<IDomainEvent>>()
            {
                {streamId.ToString(), new List<IDomainEvent> { new TestEvent(1)} }
            });
            var testEvents = new[]
            {
                new TestEvent(2),
                new TestEvent(3)
            };
            
            //Act
            await sut.StoreEventsAsync(streamId.ToString(), testEvents.Cast<IDomainEvent>().ToList(), CancellationToken.None);

            //Assert
            var events = await sut.GetByStreamIdAsync(streamId.ToString(), CancellationToken.None);
            events
                .Should()
                .HaveCount(3)
                .And
                .AllBeOfType<TestEvent>()
                .And
                .Subject
                .Cast<TestEvent>()
                .Should()
                .BeInAscendingOrder(e => e.Number);
        }
        
        [Fact]
        public async Task GetByStreamIdAsync_WithExistingAggregate_ReturnsEvents()
        {
            //Arrange
            var streamId = Guid.NewGuid();
            var sut = CreateSut(new Dictionary<string, List<IDomainEvent>>()
            {
                {streamId.ToString(), new List<IDomainEvent> { new TestEvent(1)} }
            });
            
            //Act
            var events = await sut.GetByStreamIdAsync(streamId.ToString(), CancellationToken.None);

            //Assert
            events
                .Should()
                .HaveCount(1)
                .And
                .ContainSingle(e => e.As<TestEvent>().Number == 1);
        }

        [Fact]
        public async Task GetByStreamIdAsync_WithNonExistingAggregate_Throws()
        {
            //Arrange
            var streamId = Guid.NewGuid();
            var sut = CreateSut();

            //Act
            Func<Task> action = () => sut.GetByStreamIdAsync(streamId.ToString(), CancellationToken.None);

            //Assert
            await action
                .Should()
                .ThrowAsync<Exception>();
        }


        private InMemoryEventStore CreateSut()
        {
            return new InMemoryEventStore();
        }
        
        private InMemoryEventStore CreateSut(Dictionary<string, List<IDomainEvent>> dictionary)
        {
            return new InMemoryEventStore(dictionary);
        }

        private class TestEvent : DomainEvent
        {
            public int Number { get; }

            public TestEvent(int number)
            {
                Number = number;
            }
        } 
    }
}