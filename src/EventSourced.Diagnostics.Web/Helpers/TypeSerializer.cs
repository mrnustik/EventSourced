using System;

namespace EventSourced.Diagnostics.Web.Helpers
{
    public static class TypeSerializer
    {
        public static string SerializeType(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        public static Type DeserializeType(string serializedType)
        {
            return Type.GetType(serializedType)!;
        }
    }
}