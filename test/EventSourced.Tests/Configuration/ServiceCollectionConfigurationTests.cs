using System;
using EventSourced.Configuration;
using EventSourced.Domain;
using EventSourced.Persistence;
using EventSourced.Persistence.InMemory.Configuration;
using FluentAssertions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Tests.Configuration
{
    public class ServiceCollectionConfigurationTests
    {
        [Fact]
        public void AddEventSourced_WithInMemoryDatabase_AllowsResolvingOfRepository()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();

            //Act
            serviceCollection.AddEventSourced(options => options.UseInMemoryEventStore());

            //Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var repository = serviceProvider.GetRequiredService<IRepository<TestAggregateRoot, Guid>>();
            repository
                .Should()
                .NotBeNull();
        }
        
        private class TestAggregateRoot : AggregateRoot<Guid>
        {
            public TestAggregateRoot() : base(Guid.NewGuid())
            {
            }
            
            public TestAggregateRoot(Guid id) : base(id)
            {
            }
        }
    }
}