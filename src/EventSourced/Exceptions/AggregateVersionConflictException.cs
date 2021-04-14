using System;
using System.Runtime.Serialization;

namespace EventSourced.Exceptions
{
    [Serializable]
    public class AggregateVersionConflictException : Exception
    {
        public AggregateVersionConflictException(string message)
            : base(message)
        {
        }

        protected AggregateVersionConflictException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}