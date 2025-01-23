using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserAPI.DbContext;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<ApplicationUser>(options);

public class ApplicationUser : IdentityUser
{
    // Add additional properties if needed
    public string FullName { get; set; } = string.Empty;
}