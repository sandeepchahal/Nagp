using UserAPI.DbContext;

namespace UserAPI.Helpers;

public interface IJwtTokenGeneration
{
    string? GenerateToken(ApplicationUser user);
}