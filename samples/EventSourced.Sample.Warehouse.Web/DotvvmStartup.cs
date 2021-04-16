using DotVVM.Framework.Configuration;
using DotVVM.Framework.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Sample.Warehouse.Web
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Default", "", "Pages/Default/Default.dothtml");   
            config.AutoRegisterRoutes("Pages");
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            
        }
    }
}