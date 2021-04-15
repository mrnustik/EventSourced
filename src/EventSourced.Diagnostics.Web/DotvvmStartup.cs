using DotVVM.Framework.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Diagnostics.Web
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Diagnostics_AggregateTypesList", "_diagnostics/EventSourced/AggregateTypesList", "embedded://EventSourced.Diagnostics.Web/Pages.AggregateTypes.AggregateTypesList.dothtml");
            config.RouteTable.Add("Diagnostics_AggregatesList", "_diagnostics/EventSourced/AggregatesList/{AggregateType}", "embedded://EventSourced.Diagnostics.Web/Pages.AggregatesList.AggregatesList.dothtml");
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
        }
    }
}