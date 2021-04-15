using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace EventSourced.Persistence.EntityFramework.Helpers
{
    internal class JsonSerializerSettingsProvider
    {
        public static JsonSerializerSettings Settings =>
            new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterAndCtorContractResolver()
            };
    }
}