using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Diagnostics.Web.Configuration;
using EventSourced.ExternalEvents.API.Configuration;
using EventSourced.Persistence.EntityFramework.Configuration;
using EventSourced.Persistence.InMemory.Configuration;
using EventSourced.Sample.Warehouse.Application.Configuration;
using EventSourced.Sample.Warehouse.Domain.Configuration;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;
using EventSourced.Sample.Warehouse.Web.HostedServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourced.Sample.Warehouse.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDotVVM<DotvvmStartup>();
            services.AddApplicationServices();
            services.AddEventSourced(options => options
                                                .AddEntityFrameworkSupport(
                                                    o => o.UseSqlServer(Configuration.GetConnectionString("EventStore")))
                                                .UseEntityFrameworkEventStore()
                                                .UseInMemoryProjectionStore()
                                                .UseEntityFrameworkSnapshotStore()
                                                .UseEventCountBasedSnapshotStrategy(1)
                                                .ConfigureDomainObjects())
                    .AddEventSourcedDiagnostics(new EventSourcedDiagnosticsOptions())
                    .AddEventSourcedExternalEventsWebApi(new EventSourcedExternalWebApiOptions("/EventSourced/ExternalEvents"));
            services.AddHostedService<CreateImportLocationHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseEventSourcedExternalEventsWebApi();
            app.UseDotVVM<DotvvmStartup>();
        }
    }
}