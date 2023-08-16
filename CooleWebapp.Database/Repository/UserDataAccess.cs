using CooleWebapp.Database.Model;
using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CooleWebapp.Application.Users.Repository;

namespace CooleWebapp.Database.Repository;

public sealed class UserDataAccess : IUserDataAccess
{
  private readonly WebappDbContext _dbContext;

  public UserDataAccess(WebappDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct)
  {
    var res = await _dbContext.CoolUsers.AddAsync(user, ct);
    return res.Entity;
  }

  public Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct)
  {
    return _dbContext.CoolUsers.FirstOrDefaultAsync(u => u.WebappUserId == webappUserId, ct);
  }

  public IQueryable<CoolUser> GetAllUsers()
  {
    return _dbContext.CoolUsers;
  }

  public Task<CoolUser?> GetUser(ulong coolUserId, CancellationToken ct)
  {
    return _dbContext.CoolUsers.FirstOrDefaultAsync(u => u.Id == coolUserId, ct);
  }

  public IQueryable<UserWithRoles> GetUsersWithRoles()
  {
    return GetAllUsers()
      .Join(_dbContext.Users, cu => cu.WebappUserId, wu => wu.Id, (cu, wu) => new
      {
        Balance = cu.Balance,
        CoolUserId = cu.Id,
        Name = cu.Name,
        WebappUser = wu
      })
      .GroupJoin(_dbContext.UserRoles, u => u.WebappUser.Id, r => r.UserId, (u, roles) => new UserWithRoles
      {
        Balance = u.Balance,
        CoolUserId = u.CoolUserId,
        Name = u.Name,
        Roles = roles.Join(_dbContext.Roles, r => r.RoleId, r => r.Id, (_, r) => r.NormalizedName).ToArray()
      });
  }
}
