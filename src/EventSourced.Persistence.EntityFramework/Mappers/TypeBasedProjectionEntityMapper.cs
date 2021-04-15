using EventSourced.Persistence.EntityFramework.Entities;
using EventSourced.Persistence.EntityFramework.Helpers;
using Newtonsoft.Json;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public class TypeBasedProjectionEntityMapper : ITypeBasedProjectionEntityMapper
    {
        private readonly ITypeSerializer _typeSerializer;

        public TypeBasedProjectionEntityMapper(ITypeSerializer typeSerializer)
        {
            _typeSerializer = typeSerializer;
        }

        public TypeBasedProjectionEntity MapToEntity(object projection)
        {
            var serializedType = _typeSerializer.SerializeType(projection.GetType());
            return new TypeBasedProjectionEntity
            {
                SerializedProjectionType = serializedType,
                SerializedProjectionData = JsonConvert.SerializeObject(projection, JsonSerializerSettingsProvider.Settings)
            };
        }

        public object MapToProjection(TypeBasedProjectionEntity entity)
        {
            var deserializedType = _typeSerializer.DeserializeType(entity.SerializedProjectionType);
            return JsonConvert.DeserializeObject(entity.SerializedProjectionData,
                                                 deserializedType,
                                                 JsonSerializerSettingsProvider.Settings)!;
        }
    }
}