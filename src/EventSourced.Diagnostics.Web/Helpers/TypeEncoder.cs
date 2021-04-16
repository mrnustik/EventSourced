using System;

namespace EventSourced.Diagnostics.Web.Helpers
{
    public static class TypeEncoder
    {
        public static string EncodeType(Type type)
        {
            var serializedType = TypeSerializer.SerializeType(type);
            var encodedType = Base64Encoder.Encode(serializedType);
            return encodedType;
        }
        
        public static Type DecodeType(string encodedType)
        {
            var serializedType = Base64Encoder.Decode(encodedType);
            var decodedType = TypeSerializer.DeserializeType(serializedType);
            return decodedType;
        }
    }
}