using Microsoft.EntityFrameworkCore;
using UserAPI.DbContext;

namespace UserAPI.ServiceRegistrations;

public static class ConfigureMigration
{
    public static void ConfigureMigrationServices(WebApplication app)
    {
        // Other service configurations (e.g., AddDbContext)
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        context.Database.Migrate();  
    }
}