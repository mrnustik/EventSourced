using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventSourced.Persistence.InMemory.Helpers
{
    internal static class CloningHelper
    {
        public static TObject DeepCloneGeneric<TObject>(this TObject obj) => (TObject) DeepClone(obj);
        
        public static object DeepClone(this object obj)
        {
            var type = obj.GetType();
            var serializedObject = JsonConvert.SerializeObject(obj);
            var deserializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            
            return JsonConvert.DeserializeObject(serializedObject, type, deserializeSettings)!;
        }
    }
}