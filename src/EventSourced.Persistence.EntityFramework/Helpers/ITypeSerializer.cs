using System;

namespace EventSourced.Persistence.EntityFramework.Helpers
{
    public interface ITypeSerializer
    {
        string SerializeType(Type type);
        Type DeserializeType(string typeFullName);
    }
}    
