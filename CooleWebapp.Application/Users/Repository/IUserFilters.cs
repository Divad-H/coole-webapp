using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Repository;

public interface IUserFilters
{
  Task<IQueryable<CoolUser>> FilterRegisteredUsersWithUserRole(IQueryable<CoolUser> users);
}
