using EventSourced.Persistence.EntityFramework.Entities;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public interface ITypeBaseProjectionEntityMapper
    {
        TypeBasedProjectionEntity MapToEntity(object projection);
        object MapToProjection(TypeBasedProjectionEntity entity);
    }
}