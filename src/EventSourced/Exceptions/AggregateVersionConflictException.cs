using System;
using System.Runtime.Serialization;

namespace EventSourced.Exceptions
{
    [Serializable]
    public class AggregateVersionConflictException : Exception
    {
        public AggregateVersionConflictException()
        {
        }

        public AggregateVersionConflictException(string message) : base(message)
        {
        }

        public AggregateVersionConflictException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AggregateVersionConflictException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}