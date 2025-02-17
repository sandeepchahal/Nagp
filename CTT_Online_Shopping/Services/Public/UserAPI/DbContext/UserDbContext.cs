using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserAPI.DbContext;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");

            foreach (var property in entry.Properties.Where(p => p.IsModified || entry.State == EntityState.Added))
            {
                Console.WriteLine($"Property: {property.Metadata.Name}, Old: {property.OriginalValue}, New: {property.CurrentValue}");
            }
        }
        return base.SaveChanges();
    }
}

public class ApplicationUser : IdentityUser
{
    // Add additional properties if needed
   
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Provider { get; set; } = "System";
    public string? ProviderId { get; set; } // Google User ID
}