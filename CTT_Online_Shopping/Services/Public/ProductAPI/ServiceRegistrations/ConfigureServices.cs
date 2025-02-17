using ProductAPI.DbServices;

namespace ProductAPI.ServiceRegistrations;

public static class ConfigureServices
{
    public static void ConfigureDbServices(this IServiceCollection serviceCollection)
    {
        
        serviceCollection.AddScoped<IBrandDbService, BrandDbService>();
        serviceCollection.AddScoped<ICategoryDbService, CategoryDbService>();
        serviceCollection.AddScoped<IProductDbService, ProductDbService>();
        serviceCollection.AddScoped<IProductItemDbService, ProductItemDbService>();
        serviceCollection.AddScoped<IReviewDbService, ReviewDbService>();

    }
}