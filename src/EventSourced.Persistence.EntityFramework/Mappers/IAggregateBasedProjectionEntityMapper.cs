﻿using System;
using EventSourced.Persistence.EntityFramework.Entities;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public interface IAggregateBasedProjectionEntityMapper
    {
        AggregateBasedProjectionEntity MapToEntity(Guid aggregateRootId, object projection);
        object MapToProjection(AggregateBasedProjectionEntity entity);
    }
}