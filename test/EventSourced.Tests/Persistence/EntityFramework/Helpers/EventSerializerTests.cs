using System;
using EventSourced.Domain.Events;
using EventSourced.Persistence.EntityFramework.Helpers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.EntityFramework.Helpers
{
    public class EventSerializerTests
    {
        [Fact]
        public void SerializedEvent_CanBeDeserialized()
        {
            //Arrange
            var testEvent = new TestEvent("Testing text");
            var sut = CreateSut();

            //Act
            var serializedEvent = sut.SerializeEvent(testEvent);
            var deserializedEvent = sut.DeserializeEvent(serializedEvent, typeof(TestEvent));

            //Assert
            deserializedEvent.Should()
                             .BeOfType<TestEvent>()
                             .Which.TestText.Should()
                             .Be("Testing text");
        }

        private IEventSerializer CreateSut()
        {
            return new EventSerializer();
        }

        private class TestEvent : DomainEvent
        {
            public string TestText { get; }

            public TestEvent(string testText) 
            {
                TestText = testText;
            }
        }
    }
}