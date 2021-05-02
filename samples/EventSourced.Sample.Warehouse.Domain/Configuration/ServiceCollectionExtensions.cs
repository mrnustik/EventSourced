using EventSourced.Sample.Warehouse.Domain.Container.Services;
using EventSourced.Sample.Warehouse.Domain.ImportLocation.Services;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Sample.Warehouse.Domain.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICreateImportLocationDomainService, CreateImportLocationDomainService>();
            serviceCollection.AddTransient<IMoveItemBetweenContainersService, MoveItemBetweenContainersService>();
            serviceCollection.AddTransient<IWarehouseItemTitleService, WarehouseItemTitleService>();
            return serviceCollection;
        }
    }
}