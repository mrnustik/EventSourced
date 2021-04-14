using System;

namespace EventSourced.Persistence.EntityFramework.Helpers
{
    public class TypeSerializer : ITypeSerializer
    {
        public string SerializeType(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        public Type DeserializeType(string typeFullName)
        {
            return Type.GetType(typeFullName)!;
        }
    }
}