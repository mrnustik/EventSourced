using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EventSourced.Exceptions;
using EventSourced.ExternalEvents.API.Configuration;
using EventSourced.ExternalEvents.API.Requests;
using EventSourced.ExternalEvents.API.Responses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EventSourced.ExternalEvents.API.Middleware
{
    public class ExternalEventsHandlingMiddleware : IMiddleware
    {
        private readonly EventSourcedExternalWebApiOptions _eventSourcedExternalWebApiOptions;
        private readonly IExternalEventPublisher _externalEventPublisher;
        private readonly IAuthorizationHandler _authorizationHandler;

        public ExternalEventsHandlingMiddleware(EventSourcedExternalWebApiOptions eventSourcedExternalWebApiOptions, IExternalEventPublisher externalEventPublisher, IAuthorizationHandler authorizationHandler)
        {
            _eventSourcedExternalWebApiOptions = eventSourcedExternalWebApiOptions;
            _externalEventPublisher = externalEventPublisher;
            _authorizationHandler = authorizationHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments(_eventSourcedExternalWebApiOptions.ExternalEventsEndpoint, StringComparison.Ordinal))
            {
                var cancellationToken = context.RequestAborted;
                
                if (!await _authorizationHandler.AuthorizeAsync(context, cancellationToken))
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    await ReturnErrorAsync(context, new ErrorResponse("Authorization failed"));
                    return;
                }

                if (context.Request.Method != "POST")
                {
                    context.Response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
                    await ReturnErrorAsync(context, new ErrorResponse("Only POST method is supported"));
                    return;
                }

                var deserializedEvent = await DeserializeExternalEventAsync(context);
                if (deserializedEvent == null)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await ReturnErrorAsync(context, new ErrorResponse("External event could not be deserialized."));
                    return;
                }
                try
                {
                    await _externalEventPublisher.PublishExternalEventAsync(deserializedEvent.EventType,
                                                                            deserializedEvent.EventData.ToString(),
                                                                            cancellationToken);
                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                }
                catch (ExternalEventNotRegisteredException exception)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    await ReturnErrorAsync(context, new ErrorResponse(exception.Message));
                }

                return;
            }

            await next(context);
        }

        private async Task<PublishExternalEventRequest?> DeserializeExternalEventAsync(HttpContext context)
        {
            using var streamReader = new StreamReader(context.Request.Body);
            var requestBody = await streamReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<PublishExternalEventRequest>(requestBody);
        }

        private static async Task ReturnErrorAsync(HttpContext context, ErrorResponse? errorResponse)
        {
            await using var streamWriter = new StreamWriter(context.Response.Body);
            var serializedResponse = JsonConvert.SerializeObject(errorResponse);
            await streamWriter.WriteAsync(serializedResponse);
        }
    }
}