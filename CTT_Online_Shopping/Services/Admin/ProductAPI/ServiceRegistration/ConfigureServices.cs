using ProductAPI.DbServices;
using ProductAPI.Events;

namespace ProductAPI.ServiceRegistration;

public static class ConfigureServices
{
    public static void AddScopedServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductEventService, ProductEventService>();
        serviceCollection.AddScoped<IProductItemEventService, ProductItemEventService>();

    }
    public static void ConfigureDbServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICategoryDbService, CategoryDbService>();
        serviceCollection.AddScoped<IProductDbService, ProductDbService>();
        serviceCollection.AddScoped<IProductItemDbService, ProductItemDbService>();
        serviceCollection.AddScoped<IBrandDbService, BrandDbService>();
    }
}