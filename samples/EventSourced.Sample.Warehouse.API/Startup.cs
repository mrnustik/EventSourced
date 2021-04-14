using EventSourced.Configuration;
using EventSourced.Persistence.InMemory.Configuration;
using EventSourced.Sample.Warehouse.Application.Configuration;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EventSourced.Sample.Warehouse.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "EventSourced.Sample.Warehouse.API", Version = "v1"});
            });

            services.AddApplicationServices();
            services.AddEventSourced(options => options.UseInMemoryEventStore()
                                                       .UseInMemoryProjectionStore()
                                                       .RegisterAutomaticProjection<WarehouseItemsCountProjection>()
                                                       .RegisterAutomaticAggregateProjection<WarehouseItemDetailProjection,
                                                           WarehouseItemAggregateRoot>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventSourced.Sample.Warehouse.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}