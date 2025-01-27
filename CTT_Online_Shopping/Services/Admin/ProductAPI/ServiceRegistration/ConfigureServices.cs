using ProductAPI.Events;

namespace ProductAPI.ServiceRegistration;

public static class ConfigureServices
{
    public static void AddScopedServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductEvent, ProductEvent>();
    }
}