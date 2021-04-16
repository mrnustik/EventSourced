using DotVVM.Diagnostics.StatusPage;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Sample.Warehouse.Web
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureResources(config);
            ConfigureRoutes(config);
        }

        private static void ConfigureRoutes(DotvvmConfiguration config)
        {
            config.RouteTable.Add("Default", "", "Pages/Default/Default.dothtml");
            config.RouteTable.Add("Pages/Items/Edit/Edit", "Pages/Items/Edit/Edit/{WarehouseItemId?:guid}", "Pages/Items/Edit/Edit.dothtml");
            config.AutoRegisterRoutes("Pages");
        }

        private static void ConfigureResources(DotvvmConfiguration config)
        {
            config.Resources.Register("bootstrap-css",
                                      new StylesheetResource(
                                          new UrlResourceLocation(
                                              "https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css")));
            config.Resources.Register("popper-js",
                                      new ScriptResource(
                                          new UrlResourceLocation(
                                              "https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.1/dist/umd/popper.min.js")));
            config.Resources.Register("bootstrap-js",
                                      new ScriptResource(new UrlResourceLocation(
                                                             "https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/js/bootstrap.bundle.min.js"))
                                      {
                                          Dependencies = new[] {"popper-js"}
                                      });
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.AddStatusPage();
        }
    }
}