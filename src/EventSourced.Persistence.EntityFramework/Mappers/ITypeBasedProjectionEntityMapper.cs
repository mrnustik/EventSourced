using EventSourced.Persistence.EntityFramework.Entities;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public interface ITypeBasedProjectionEntityMapper
    {
        TypeBasedProjectionEntity MapToEntity(object projection);
        object MapToProjection(TypeBasedProjectionEntity entity);
    }
}