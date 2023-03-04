using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Entities;
using System.Collections.Immutable;

namespace CooleWebapp.Auth.Managers;

internal class UserFilters : IUserFilters
{
  private readonly IUserManager _userManager;
  public UserFilters(IUserManager userManager)
  {
    _userManager = userManager;
  }

  public async Task<IQueryable<CoolUser>> FilterRegisteredUsersWithUserRole(IQueryable<CoolUser> users)
  {
    var webappUsers = (await _userManager.GetUsersInRoleAsync("USER"))
      .Where(u => u.EmailConfirmed).Select(u => u.Id).ToArray();
    return users.Where(cu => webappUsers.Contains(cu.WebappUserId));
  }
}
