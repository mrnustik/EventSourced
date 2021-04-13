using System.Collections.Generic;

namespace EventSourced.Helpers
{
    internal static class QueueExtensions
    {
        public static IList<TItem> DequeueAll<TItem>(this Queue<TItem> queue)
        {
            var list = new List<TItem>();
            while (queue.TryDequeue(out var result)) list.Add(result);
            return list;
        }
    }
}