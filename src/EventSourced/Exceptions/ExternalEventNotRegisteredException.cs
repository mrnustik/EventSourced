using System;
using System.Runtime.Serialization;

namespace EventSourced.Exceptions
{
    [Serializable]
    public class ExternalEventNotRegisteredException : Exception
    {
        public ExternalEventNotRegisteredException(string? message)
            : base(message)
        {
        }
        
        protected ExternalEventNotRegisteredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}