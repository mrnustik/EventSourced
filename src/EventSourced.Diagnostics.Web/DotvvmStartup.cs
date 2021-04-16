using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using EventSourced.Diagnostics.Web.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Diagnostics.Web
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config);
            ConfigureResources(config);
            config.Markup.AddCodeControls("cc", typeof(JsonViewer));
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

            config.Resources.Register("jquery-js",
                                      new ScriptResource(new UrlResourceLocation("https://code.jquery.com/jquery-3.6.0.slim.min.js")));

            config.Resources.Register("json-viewer-js",
                                      new ScriptResource(new UrlResourceLocation(
                                                             "https://cdn.jsdelivr.net/npm/jquery.json-viewer@1.4.0/json-viewer/jquery.json-viewer.js"))
                                      {
                                          Dependencies = new[] {"jquery-js"}
                                      });
            config.Resources.Register("json-viewer-css",
                                      new StylesheetResource(new UrlResourceLocation(
                                                                 "https://cdn.jsdelivr.net/npm/jquery.json-viewer@1.4.0/json-viewer/jquery.json-viewer.css")));

            config.Resources.Register("json-viewer-control-js",
                                      new ScriptResource(new EmbeddedResourceLocation(
                                                             typeof(DotvvmStartup).Assembly,
                                                             "EventSourced.Diagnostics.Web.Resources.JsonViewer.js"))
                                      {
                                          Dependencies = new[] {"json-viewer-js", "knockout"}
                                      });
            config.Resources.Register("copy-json-button-js",
                                      new ScriptResource(new EmbeddedResourceLocation(
                                                             typeof(DotvvmStartup).Assembly,
                                                             "EventSourced.Diagnostics.Web.Resources.CopyJsonButton.js"))
                                      {
                                          Dependencies = new[] {"knockout"}
                                      });
        }

        private static void ConfigureRoutes(DotvvmConfiguration config)
        {
            RegisterRoute(config,
                          "Diagnostics_AggregateTypesList",
                          "_diagnostics/EventSourced/AggregateTypesList",
                          "Pages/AggregateTypes/AggregateTypesList.dothtml");
            
            RegisterRoute(config,
                          "Diagnostics_AggregatesList",
                          "_diagnostics/EventSourced/AggregatesList/{AggregateType}",
                          "Pages/AggregatesList/AggregatesList.dothtml");
            
            RegisterRoute(config,
                          "Diagnostics_ProjectionsList",
                          "_diagnostics/EventSourced/ProjectionsList",
                          "Pages/ProjectionsList/ProjectionsList.dothtml");

            RegisterRoute(config,
                        "Diagnostics_AggregateProjectionsList",
                        "_diagnostics/EventSource/AggregateProjectionsList",
                        "Pages/AggregateProjectionsList/AggregateProjectionsList.dothtml");
            
            RegisterRoute(config,
                          "Diagnostics_AggregateProjectionTypeDetail",
                          "_diagnostics/EventSource/AggregateProjectionTypeDetail/{ProjectionType}",
                          "Pages/AggregateProjectionTypeDetail/AggregateProjectionTypeDetail.dothtml");
        }

        private static void RegisterRoute(DotvvmConfiguration config, string routeName, string url, string pagePath)
        {
            var embeddedResourcePath = pagePath.Replace('/', '.');
            config.RouteTable.Add(routeName,
                                  url,
#if DEBUG
                                  pagePath);
#else
                                  $"embedded://EventSourced.Diagnostics.Web/{embeddedResourcePath}");
#endif
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
        }
    }
}