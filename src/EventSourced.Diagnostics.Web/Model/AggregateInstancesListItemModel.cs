using System;

namespace EventSourced.Diagnostics.Web.Model
{
    public class AggregateInstancesListItemModel
    {
        public Guid Id { get; }

        public AggregateInstancesListItemModel(Guid id)
        {
            Id = id;
        }
    }
}