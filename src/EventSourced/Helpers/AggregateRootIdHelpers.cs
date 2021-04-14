using System;

namespace EventSourced.Helpers
{
    public static class AggregateRootIdHelpers
    {
        public static TAggregateRootId ToAggregateRootId<TAggregateRootId>(this string streamId)
        {
            var idType = typeof(TAggregateRootId);
            return (TAggregateRootId) streamId.ToAggregateRootId(idType);
        }

        public static object ToAggregateRootId(this string streamId, Type idType)
        {
            if (idType.IsPrimitive) return Convert.ChangeType(streamId, idType);
            return Activator.CreateInstance(idType, streamId)!;
        }
    }
}