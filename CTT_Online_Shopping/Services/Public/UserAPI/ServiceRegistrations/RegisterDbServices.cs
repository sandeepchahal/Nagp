using UserAPI.Helpers;

namespace UserAPI.ServiceRegistrations;

public static class RegisterDbServices
{

    public static void ConfigureDbServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IJwtTokenGeneration, JwtTokenGeneration>();

    }
}