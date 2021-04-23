using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Exceptions;
using EventSourced.ExternalEvents.API.Configuration;
using EventSourced.ExternalEvents.API.Requests;
using EventSourced.ExternalEvents.API.Responses;
using Microsoft.AspNetCore.Http;

namespace EventSourced.ExternalEvents.API.Middleware
{
    public class ExternalEventsHandlingMiddleware : IMiddleware
    {
        private readonly EventSourcedExternalWebApiOptions _eventSourcedExternalWebApiOptions;
        private readonly IExternalEventPublisher _externalEventPublisher;

        public ExternalEventsHandlingMiddleware(EventSourcedExternalWebApiOptions eventSourcedExternalWebApiOptions, IExternalEventPublisher externalEventPublisher)
        {
            _eventSourcedExternalWebApiOptions = eventSourcedExternalWebApiOptions;
            _externalEventPublisher = externalEventPublisher;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments(_eventSourcedExternalWebApiOptions.ExternalEventsEndpoint, StringComparison.Ordinal))
            {
                var cancellationToken = context.RequestAborted;

                if (context.Request.Method != "POST")
                {
                    context.Response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
                    await ReturnErrorAsync(context, new ErrorResponse("Only POST method is supported"), cancellationToken);
                    return;
                }
                var deserializedEvent = await JsonSerializer.DeserializeAsync<PublishExternalEventRequest>(context.Request.Body, null, cancellationToken);
                if (deserializedEvent == null)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await ReturnErrorAsync(context, new ErrorResponse("External event could not be deserialized."), cancellationToken);
                    return;
                }
                try
                {
                    await _externalEventPublisher.PublishExternalEventAsync(deserializedEvent.EventType,
                                                                            deserializedEvent.EventData,
                                                                            cancellationToken);
                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                }
                catch (ExternalEventNotRegisteredException exception)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    await ReturnErrorAsync(context, new ErrorResponse(exception.Message), cancellationToken);
                }

                return;
            }

            await next(context);
        }

        private static async Task ReturnErrorAsync(HttpContext context, ErrorResponse? errorResponse, CancellationToken cancellationToken)
        {
            await JsonSerializer.SerializeAsync(context.Response.Body, errorResponse, null, cancellationToken);
        }
    }
}