using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Exceptions;
using EventSourced.ExternalEvents.API.Configuration;
using EventSourced.ExternalEvents.API.Requests;
using EventSourced.Persistence.InMemory.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Xunit;

namespace EventSourced.ExternalEvents.API.Tests.Middleware
{
    public class ExternalEventsHandlingMiddlewareTests
    {
        [Fact]
        public async Task HandleAsync_WithoutCorrectPathString_CallsNext()
        {
            //Arrange
            var options = new EventSourcedExternalWebApiOptions("/EventSourced/ExternalEvent");
            using var webHost = await CreateWebHost(options);

            //Act
            var response = await webHost.GetTestClient()
                                        .GetAsync("/");

            //Arrange
            response.StatusCode.Should()
                    .Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HandleAsync_WithCorrectPathStringWithInvalidMethod_ReturnsMethodNotAllowed()
        {
            //Arrange
            var options = new EventSourcedExternalWebApiOptions("/EventSourced/ExternalEvent");
            using var webHost = await CreateWebHost(options);

            //Act
            var response = await webHost.GetTestClient()
                                        .GetAsync("/EventSourced/ExternalEvent");

            //Arrange
            response.StatusCode.Should()
                    .Be(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task HandleAsync_WithCorrectPathStringWithCorrectMethodWithInvalidBody_ReturnsUnprocessableEntity()
        {
            //Arrange
            var options = new EventSourcedExternalWebApiOptions("/EventSourced/ExternalEvent");
            using var webHost = await CreateWebHost(options);

            //Act
            var response = await webHost.GetTestClient()
                                        .PostAsJsonAsync<object?>("/EventSourced/ExternalEvent", null);

            //Arrange
            response.StatusCode.Should()
                    .Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task HandleAsync_WithCorrectPathStringWithCorrectMethodWithValidBodyWithUnregisteredEventType_ReturnsNotFound()
        {
            //Arrange
            var options = new EventSourcedExternalWebApiOptions("/EventSourced/ExternalEvent");
            using var webHost = await CreateWebHost(options);

            //Act
            var response = await webHost.GetTestClient()
                                        .PostAsJsonAsync("/EventSourced/ExternalEvent",
                                                         new PublishExternalEventRequest(nameof(UnregisteredEvent), new JObject()));

            //Arrange
            response.StatusCode.Should()
                    .Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HandleAsync_WithEverythingValid_ReturnsOK()
        {
            //Arrange
            var options = new EventSourcedExternalWebApiOptions("/EventSourced/ExternalEvent");
            using var webHost = await CreateWebHost(options);

            //Act
            var response = await webHost.GetTestClient()
                                        .PostAsJsonAsync("/EventSourced/ExternalEvent",
                                                         new PublishExternalEventRequest(nameof(TestEventType), new JObject()));

            //Arrange
            response.StatusCode.Should()
                    .Be(HttpStatusCode.OK);
        }

        private Task<IHost> CreateWebHost(EventSourcedExternalWebApiOptions options)
        {
            return new HostBuilder().ConfigureWebHost(webBuilder =>
                                    {
                                        webBuilder.UseTestServer()
                                                  .ConfigureServices(services =>
                                                  {
                                                      services.AddEventSourced(o => o.UseInMemoryEventStore()
                                                                                     .UseInMemoryProjectionStore()
                                                                                     .UseInMemorySnapshotStore()
                                                                                     .RegisterExternalEventHandler<TestEventType, TestEventHandler>());
                                                      services.AddEventSourcedExternalEventsWebApi(options);
                                                  })
                                                  .Configure(app => app.UseEventSourcedExternalEventsWebApi());
                                    })
                                    .StartAsync();
        }

        private class TestEventType
        {
        }

        private class UnregisteredEvent
        {
            
        }
        
        private class TestEventHandler: IExternalEventHandler<TestEventType>
        {
            public Task HandleAsync(TestEventType externalEvent, CancellationToken ct)
            {
                return Task.CompletedTask;
            }
        }
    }
}