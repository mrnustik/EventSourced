using EventSourced.Sample.Warehouse.Application.Services;
using EventSourced.Sample.Warehouse.Domain.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Sample.Warehouse.Application.Configuration
{
    public static class ServiceItemCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(s => s.FromCallingAssembly()
                                         .AddClasses(c => c.AssignableTo<ApplicationServiceBase>())
                                         .AsSelfWithInterfaces()
                                         .WithTransientLifetime());

            serviceCollection.AddDomainServices();
        }
    }
}