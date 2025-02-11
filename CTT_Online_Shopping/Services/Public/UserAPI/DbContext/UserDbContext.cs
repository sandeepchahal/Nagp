using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserAPI.DbContext;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<ApplicationUser>(options);

public class ApplicationUser : IdentityUser
{
    // Add additional properties if needed
   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Provider { get; set; }  // "Google", "Facebook"
    public string ProviderId { get; set; } // Google User ID
}