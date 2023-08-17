using CooleWebapp.Database.Model;
using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Database.Repository;

public sealed class UserDataAccess : IUserDataAccess
{
  private readonly WebappDbContext _dbContext;
  private readonly IUserManager _userManager;

  public UserDataAccess(
    WebappDbContext dbContext,
    IUserManager userManager)
  {
    _dbContext = dbContext;
    _userManager = userManager;
  }

  public async Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct)
  {
    var res = await _dbContext.CoolUsers.AddAsync(user, ct);
    return res.Entity;
  }

  public async Task DeleteUser(UInt64 coolUserId, CancellationToken ct)
  {
    var user = await _dbContext.CoolUsers.FirstOrDefaultAsync(u => u.Id == coolUserId, ct);
    if (user is null)
    {
      throw new ClientError(ErrorType.NotFound, "User does not exist.");
    }

    user.IsDeleted = true;

    if (user.WebappUserId is null)
    {
      return;
    }
    var webappUser = await _userManager.FindByIdAsync(user.WebappUserId);
    if (webappUser is not null)
    {
      await _userManager.DeleteAsync(webappUser);
    }
    user.WebappUserId = null;
  }

  public Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct)
  {
    return _dbContext.CoolUsers.FirstOrDefaultAsync(u => u.WebappUserId == webappUserId, ct);
  }

  public IQueryable<CoolUser> GetAllUsers()
  {
    return _dbContext.CoolUsers;
  }

  public Task<CoolUser?> GetUser(UInt64 coolUserId, CancellationToken ct)
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
        Email = wu.Email,
        WebappUser = wu
      })
      .GroupJoin(_dbContext.UserRoles, u => u.WebappUser.Id, r => r.UserId, (u, roles) => new UserWithRoles
      {
        Balance = u.Balance,
        CoolUserId = u.CoolUserId,
        Name = u.Name,
        Email = u.Email,
        Roles = roles.Join(_dbContext.Roles, r => r.RoleId, r => r.Id, (_, r) => r.NormalizedName).ToArray()
      });
  }

  public async Task SetUserRoles(UInt64 coolUserId, IReadOnlyCollection<string> roles, CancellationToken ct)
  {
    var user = await GetUser(coolUserId, ct) 
      ?? throw new ClientError(ErrorType.NotFound, "A user with that Id does not exist.");
    await _userManager.SetUserRoles(user.WebappUserId, roles, ct);
  }
}
