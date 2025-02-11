using UserAPI.DbContext;

namespace UserAPI.DbServices;

public interface IUserDbService
{
   Task<ApplicationUser> FindByEmailAsync(string email);
}