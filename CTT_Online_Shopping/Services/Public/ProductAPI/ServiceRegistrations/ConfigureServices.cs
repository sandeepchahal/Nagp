using ProductAPI.DbServices;

namespace ProductAPI.ServiceRegistrations;

public static class ConfigureServices
{
    public static void ConfigureDbServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductDbService, ProductDbService>();
        serviceCollection.AddScoped<IProductItemDbService, ProductItemDbService>();

    }
}