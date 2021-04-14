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
        public static TObject DeepClone<TObject>(this TObject obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj);
            var deserializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            
            return JsonConvert.DeserializeObject<TObject>(serializedObject, deserializeSettings)!;
        }
    }
}